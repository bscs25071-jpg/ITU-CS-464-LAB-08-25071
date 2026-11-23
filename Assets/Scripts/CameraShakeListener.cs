
using UnityEngine;
using System.Collections;

public class CameraShakeListener : MonoBehaviour
{
    public PlayerHealth health;
    public Transform cameraTransform;

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.15f;

    Vector3 originalPosition;

    void Start()
    {
        if (cameraTransform != null)
        {
            originalPosition = cameraTransform.localPosition;
        }
    }

    void OnEnable()
    {
        if (health != null)
            health.Damaged += Shake;
    }

    void OnDisable()
    {
        if (health != null)
            health.Damaged -= Shake;
    }

    void Shake(int currentHealth, int maxHealth)
    {
        if (cameraTransform != null)
            StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPosition;
    }
}
