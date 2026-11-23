
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitFlashListener : MonoBehaviour
{
    public PlayerHealth health;
    public Image flashImage;

    void OnEnable()
    {
        if (health != null)
            health.Damaged += Flash;
    }

    void OnDisable()
    {
        if (health != null)
            health.Damaged -= Flash;
    }

    void OnDisableFlash()
    {
        if (flashImage != null)
            flashImage.color = new Color(1, 0, 0, 0);
    }

    void Flash(int currentHealth, int maxHealth)
    {
        if (flashImage != null)
        {
            flashImage.color = new Color(1, 0, 0, 0.5f);
            CancelInvoke(nameof(OnDisableFlash));
            Invoke(nameof(OnDisableFlash), 0.2f);
        }
    }
}
