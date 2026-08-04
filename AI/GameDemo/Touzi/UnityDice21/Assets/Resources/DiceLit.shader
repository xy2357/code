Shader "Dice21/RuntimeLit"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Pass
        {
            ZWrite On
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
            };

            fixed4 _Color;

            v2f vert(appdata input)
            {
                v2f output;
                output.position = UnityObjectToClipPos(input.vertex);
                output.worldNormal = UnityObjectToWorldNormal(input.normal);
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                float3 normal = normalize(input.worldNormal);
                float3 keyDirection = normalize(float3(-0.45, 0.72, -0.65));
                float diffuse = 0.46 + 0.54 * saturate(dot(normal, keyDirection));
                float rim = pow(1.0 - saturate(abs(normal.z)), 3.0) * 0.12;
                return fixed4(_Color.rgb * diffuse + rim, _Color.a);
            }
            ENDCG
        }
    }
}
