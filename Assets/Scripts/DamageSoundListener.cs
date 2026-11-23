using UnityEngine;

public class DamageSoundListener : MonoBehaviour
{
    public PlayerHealth health;

    void OnEnable()
    {
        if (health != null)
            health.Damaged += PlayDamageSound;
    }

    void OnDisable()
    {
        if (health != null)
            health.Damaged -= PlayDamageSound;
    }

    void PlayDamageSound(int currentHealth, int maxHealth)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDamage();
    }
}