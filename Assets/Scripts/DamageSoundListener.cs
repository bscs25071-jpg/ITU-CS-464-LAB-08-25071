
using UnityEngine;

public class DamageSoundListener : MonoBehaviour
{
    public PlayerHealth health;
    public AudioSource damageSound;

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
        if (damageSound != null)
            damageSound.Play();
    }
}
