using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    void OnEnable()
    {
        transform.localScale = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(0.92f, 0.08f, Ease.OutCubic));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(1f, 0.25f, Ease.OutBack));
    }

    IEnumerator ScaleTo(float target, float dur, Func<float, float> ease)
    {
        float start = transform.localScale.x;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / dur;
            float k = ease(Mathf.Clamp01(t));
            transform.localScale = Vector3.one * Mathf.LerpUnclamped(start, target, k);
            yield return null;
        }
        transform.localScale = Vector3.one * target;
    }
}
