using UnityEngine;

public static class Ease
{
    public static float OutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }

    public static float InCubic(float t)
    {
        return t * t * t;
    }

    public static float InOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
    }

    public static float OutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}
