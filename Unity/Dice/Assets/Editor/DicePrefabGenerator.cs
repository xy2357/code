using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DicePrefabGenerator
{
    private const string MeshPath = "Assets/Prefab/Dice3D_RoundedMesh.asset";
    private const string PrefabPath = "Assets/Prefab/Dice3D.prefab";
    private const string BodyMaterialPath = "Assets/Material/Dice3D_Body.mat";
    private const string PipMaterialPath = "Assets/Material/Dice3D_Pips.mat";

    [MenuItem("Tools/Dice/Generate 3D Dice Prefab")]
    public static void Generate()
    {
        EnsureFolder("Assets/Editor");
        EnsureFolder("Assets/Prefab");
        EnsureFolder("Assets/Material");

        Mesh mesh = CreateRoundedCubeMesh(0.5f, 0.12f, 12);
        mesh.name = "Dice3D_RoundedMesh";

        Mesh existingMesh = AssetDatabase.LoadAssetAtPath<Mesh>(MeshPath);
        if (existingMesh == null)
        {
            AssetDatabase.CreateAsset(mesh, MeshPath);
        }
        else
        {
            EditorUtility.CopySerialized(mesh, existingMesh);
            Object.DestroyImmediate(mesh);
            mesh = existingMesh;
            EditorUtility.SetDirty(mesh);
        }

        Material bodyMaterial = GetOrCreateMaterial(
            BodyMaterialPath,
            new Color(0.96f, 0.96f, 0.93f, 1f),
            0.48f);
        Material pipMaterial = GetOrCreateMaterial(
            PipMaterialPath,
            new Color(0.018f, 0.018f, 0.022f, 1f),
            0.32f);

        GameObject root = new GameObject("Dice3D");
        try
        {
            MeshFilter filter = root.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;

            MeshRenderer renderer = root.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = bodyMaterial;

            BoxCollider collider = root.AddComponent<BoxCollider>();
            collider.size = Vector3.one * 0.96f;

            Rigidbody rigidbody = root.AddComponent<Rigidbody>();
            rigidbody.mass = 1f;
            rigidbody.linearDamping = 0f;
            rigidbody.angularDamping = 0.05f;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            root.AddComponent<DiceDice>();

            GameObject pips = new GameObject("Pips");
            pips.transform.SetParent(root.transform, false);

            AddFacePips(pips.transform, 1, Vector3.up, Vector3.right, Vector3.forward, pipMaterial);
            AddFacePips(pips.transform, 6, Vector3.down, Vector3.right, Vector3.forward, pipMaterial);
            AddFacePips(pips.transform, 2, Vector3.forward, Vector3.right, Vector3.up, pipMaterial);
            AddFacePips(pips.transform, 5, Vector3.back, Vector3.left, Vector3.up, pipMaterial);
            AddFacePips(pips.transform, 3, Vector3.right, Vector3.back, Vector3.up, pipMaterial);
            AddFacePips(pips.transform, 4, Vector3.left, Vector3.forward, Vector3.up, pipMaterial);

            PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
        }
        finally
        {
            Object.DestroyImmediate(root);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"3D dice prefab generated at {PrefabPath}");
    }

    private static Mesh CreateRoundedCubeMesh(float halfSize, float radius, int segments)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        AddRoundedFace(vertices, normals, uvs, triangles, Vector3.right, Vector3.back, Vector3.up, halfSize, radius, segments);
        AddRoundedFace(vertices, normals, uvs, triangles, Vector3.left, Vector3.forward, Vector3.up, halfSize, radius, segments);
        AddRoundedFace(vertices, normals, uvs, triangles, Vector3.up, Vector3.right, Vector3.back, halfSize, radius, segments);
        AddRoundedFace(vertices, normals, uvs, triangles, Vector3.down, Vector3.right, Vector3.forward, halfSize, radius, segments);
        AddRoundedFace(vertices, normals, uvs, triangles, Vector3.forward, Vector3.right, Vector3.up, halfSize, radius, segments);
        AddRoundedFace(vertices, normals, uvs, triangles, Vector3.back, Vector3.left, Vector3.up, halfSize, radius, segments);

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        return mesh;
    }

    private static void AddRoundedFace(
        List<Vector3> vertices,
        List<Vector3> normals,
        List<Vector2> uvs,
        List<int> triangles,
        Vector3 faceNormal,
        Vector3 horizontal,
        Vector3 vertical,
        float halfSize,
        float radius,
        int segments)
    {
        int firstVertex = vertices.Count;
        float innerExtent = halfSize - radius;

        for (int y = 0; y <= segments; y++)
        {
            float v = y / (float)segments;
            float py = Mathf.Lerp(-halfSize, halfSize, v);

            for (int x = 0; x <= segments; x++)
            {
                float u = x / (float)segments;
                float px = Mathf.Lerp(-halfSize, halfSize, u);
                Vector3 source = faceNormal * halfSize + horizontal * px + vertical * py;
                Vector3 inner = new Vector3(
                    Mathf.Clamp(source.x, -innerExtent, innerExtent),
                    Mathf.Clamp(source.y, -innerExtent, innerExtent),
                    Mathf.Clamp(source.z, -innerExtent, innerExtent));
                Vector3 normal = (source - inner).normalized;

                vertices.Add(inner + normal * radius);
                normals.Add(normal);
                uvs.Add(new Vector2(u, v));
            }
        }

        int rowLength = segments + 1;
        for (int y = 0; y < segments; y++)
        {
            for (int x = 0; x < segments; x++)
            {
                int a = firstVertex + y * rowLength + x;
                int b = a + 1;
                int c = a + rowLength;
                int d = c + 1;
                triangles.Add(a);
                triangles.Add(b);
                triangles.Add(c);
                triangles.Add(b);
                triangles.Add(d);
                triangles.Add(c);
            }
        }
    }

    private static void AddFacePips(
        Transform parent,
        int value,
        Vector3 normal,
        Vector3 horizontal,
        Vector3 vertical,
        Material material)
    {
        Vector2[] positions = GetPipPositions(value);
        const float spacing = 0.22f;

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject pip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pip.name = $"Face{value}_Pip{i + 1:00}";
            pip.transform.SetParent(parent, false);
            pip.transform.localPosition = normal * 0.493f
                + horizontal * positions[i].x * spacing
                + vertical * positions[i].y * spacing;
            pip.transform.localRotation = Quaternion.FromToRotation(Vector3.up, normal);
            pip.transform.localScale = new Vector3(0.155f, 0.038f, 0.155f);
            pip.GetComponent<MeshRenderer>().sharedMaterial = material;

            Collider pipCollider = pip.GetComponent<Collider>();
            if (pipCollider != null)
            {
                Object.DestroyImmediate(pipCollider);
            }
        }
    }

    private static Vector2[] GetPipPositions(int value)
    {
        Vector2 topLeft = new Vector2(-1f, 1f);
        Vector2 topRight = new Vector2(1f, 1f);
        Vector2 middleLeft = new Vector2(-1f, 0f);
        Vector2 middleRight = new Vector2(1f, 0f);
        Vector2 bottomLeft = new Vector2(-1f, -1f);
        Vector2 bottomRight = new Vector2(1f, -1f);
        Vector2 center = Vector2.zero;

        switch (value)
        {
            case 1: return new[] { center };
            case 2: return new[] { topLeft, bottomRight };
            case 3: return new[] { topLeft, center, bottomRight };
            case 4: return new[] { topLeft, topRight, bottomLeft, bottomRight };
            case 5: return new[] { topLeft, topRight, center, bottomLeft, bottomRight };
            case 6: return new[] { topLeft, middleLeft, bottomLeft, topRight, middleRight, bottomRight };
            default: return new Vector2[0];
        }
    }

    private static Material GetOrCreateMaterial(string path, Color color, float smoothness)
    {
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            material = new Material(shader);
            AssetDatabase.CreateAsset(material, path);
        }

        material.color = color;
        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }
        if (material.HasProperty("_Smoothness"))
        {
            material.SetFloat("_Smoothness", smoothness);
        }
        EditorUtility.SetDirty(material);
        return material;
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
        {
            return;
        }

        int separator = path.LastIndexOf('/');
        string parent = path.Substring(0, separator);
        string folder = path.Substring(separator + 1);
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, folder);
    }
}
