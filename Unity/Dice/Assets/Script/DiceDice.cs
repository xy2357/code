using System.Collections;
using UnityEngine;

public class DiceDice : MonoBehaviour
{
    [Header("动画参数")]
    [SerializeField] private float rollDuration = 1.2f;
    [SerializeField] private float throwHeight = 2.5f;
    [SerializeField] private float moveDistance = 2f;
    [SerializeField, Min(1)] private int spinTurns = 3;

    [Header("落地参数")]
    [SerializeField] private float bounceHeight = 0.3f;
    [SerializeField] private float bounceDuration = 0.25f;

    private bool isRolling;
    private Vector3 rollDirection;
    private Rigidbody diceRigidbody;
    private bool wasKinematic;

    private void Awake()
    {
        diceRigidbody = GetComponent<Rigidbody>();

        // 结算会改变骰子的朝向，所以投掷方向要独立保存。
        rollDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        if (rollDirection.sqrMagnitude < 0.001f)
        {
            rollDirection = Vector3.forward;
        }
    }

    /// <summary>
    /// 投掷指定点数，范围为 1~6。
    /// </summary>
    public void Roll(int result)
    {
        if (isRolling)
        {
            return;
        }

        if (result < 1 || result > 6)
        {
            Debug.LogError("骰子结果必须是 1~6");
            return;
        }

        StartCoroutine(RollCoroutine(result));
    }

    /// <summary>
    /// 随机投骰子。
    /// </summary>
    public void RollRandom()
    {
        // 动画中不再生成或打印一个实际不会执行的新结果。
        if (isRolling)
        {
            return;
        }

        int result = Random.Range(1, 7);
        Debug.Log("骰子结果：" + result);
        Roll(result);
    }

    private IEnumerator RollCoroutine(int result)
    {
        isRolling = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + rollDirection * moveDistance;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = GetClosestResultRotation(result, startRotation);
        Vector3 spinAxis = new Vector3(1f, 0.7f, 0.8f).normalized;

        BeginScriptedMotion();

        float duration = Mathf.Max(rollDuration, 0.01f);
        float timer = 0f;

        // 第一阶段：抛起和快速翻滚。
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            Vector3 position = Vector3.Lerp(startPosition, endPosition, t);
            position.y += 4f * throwHeight * t * (1f - t);
            transform.position = position;

            // 完整圈翻滚在终点会回到单位旋转，因此可以无缝落到目标姿态。
            // SmootherStep 让翻滚自然加速和减速，落地时角速度为 0。
            float spinProgress = SmootherStep(t);
            Quaternion spinRotation =
                Quaternion.AngleAxis(spinTurns * 360f * spinProgress, spinAxis);

            // 只在后半段逐渐对准结果，此时骰子仍在空中，不会穿过地面。
            float settleProgress = SmootherStep(Mathf.InverseLerp(0.45f, 1f, t));
            Quaternion settlingRotation =
                Quaternion.Slerp(startRotation, targetRotation, settleProgress);

            transform.rotation = spinRotation * settlingRotation;

            yield return null;
        }

        transform.position = endPosition;
        transform.rotation = targetRotation;

        // 第二阶段：此时已经是正确点数，只做轻微弹跳。
        yield return StartCoroutine(BounceCoroutine(endPosition));

        transform.rotation = targetRotation;

        EndScriptedMotion();
        isRolling = false;
        Debug.Log("骰子动画结束，结果：" + result);
    }

    private IEnumerator BounceCoroutine(Vector3 groundPosition)
    {
        float duration = Mathf.Max(bounceDuration, 0.01f);
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            Vector3 position = groundPosition;
            position.y += 4f * bounceHeight * t * (1f - t);
            transform.position = position;

            yield return null;
        }

        transform.position = groundPosition;
    }

    private static float SmootherStep(float value)
    {
        float t = Mathf.Clamp01(value);
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }

    private void BeginScriptedMotion()
    {
        if (diceRigidbody == null)
        {
            return;
        }

        wasKinematic = diceRigidbody.isKinematic;
        if (!wasKinematic)
        {
            diceRigidbody.linearVelocity = Vector3.zero;
            diceRigidbody.angularVelocity = Vector3.zero;
        }

        // 动画直接驱动 Transform，期间暂停物理解算以避免抖动。
        diceRigidbody.isKinematic = true;
    }

    private void EndScriptedMotion()
    {
        if (diceRigidbody != null)
        {
            diceRigidbody.isKinematic = wasKinematic;
        }
    }

    /// <summary>
    /// 根据 DicePrefabGenerator 中定义的六个面，返回目标点数朝上的基准姿态。
    /// </summary>
    private Quaternion GetResultRotation(int result)
    {
        switch (result)
        {
            case 1:
                return Quaternion.identity;
            case 2:
                return Quaternion.Euler(-90f, 0f, 0f);
            case 3:
                return Quaternion.Euler(0f, 0f, 90f);
            case 4:
                return Quaternion.Euler(0f, 0f, -90f);
            case 5:
                return Quaternion.Euler(90f, 0f, 0f);
            case 6:
                return Quaternion.Euler(180f, 0f, 0f);
            default:
                return Quaternion.identity;
        }
    }

    private Quaternion GetClosestResultRotation(int result, Quaternion currentRotation)
    {
        Quaternion baseRotation = GetResultRotation(result);
        Quaternion closestRotation = baseRotation;
        float smallestAngle = Quaternion.Angle(currentRotation, baseRotation);

        for (int quarterTurn = 1; quarterTurn < 4; quarterTurn++)
        {
            Quaternion candidate =
                Quaternion.AngleAxis(quarterTurn * 90f, Vector3.up) * baseRotation;
            float angle = Quaternion.Angle(currentRotation, candidate);

            if (angle < smallestAngle)
            {
                smallestAngle = angle;
                closestRotation = candidate;
            }
        }

        return closestRotation;
    }
}
