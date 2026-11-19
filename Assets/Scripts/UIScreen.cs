using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour
{
    public float duration = 0.35f;
    public bool popStyle = false;

    CanvasGroup cg;
    RectTransform rt;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        rt = GetComponent<RectTransform>();
    }

    public void Show(bool forward)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(Animate(true, forward));
    }

    public void Hide(bool forward)
    {
        if (!gameObject.activeSelf) return;
        StopAllCoroutines();
        StartCoroutine(Animate(false, forward));
    }

    IEnumerator Animate(bool show, bool forward)
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;

        float width = ((RectTransform)rt.parent).rect.width;
        float offset = width * 0.3f;
        float dir = forward ? 1f : -1f;

        float startAlpha = show ? 0f : 1f;
        float endAlpha = show ? 1f : 0f;

        float startX = show ? offset * dir : 0f;
        float endX = show ? 0f : -offset * dir;

        float startScale = show ? 0.85f : 1f;
        float endScale = show ? 1f : 0.85f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            float k = Mathf.Clamp01(t);

            if (popStyle)
            {
                float e = show ? Ease.OutBack(k) : Ease.InCubic(k);
                rt.localScale = Vector3.one * Mathf.LerpUnclamped(startScale, endScale, e);
                cg.alpha = Mathf.Lerp(startAlpha, endAlpha, Ease.InOutQuad(k));
            }
            else
            {
                float e = show ? Ease.OutCubic(k) : Ease.InCubic(k);
                rt.anchoredPosition = new Vector2(Mathf.Lerp(startX, endX, e), 0f);
                cg.alpha = Mathf.Lerp(startAlpha, endAlpha, Ease.InOutQuad(k));
            }
            yield return null;
        }

        cg.alpha = endAlpha;
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        if (show)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}