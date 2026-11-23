
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth health;
    public Image healthBar;

    void OnEnable()
    {
        if (health != null)
            health.Damaged += OnDamaged;
    }

    void OnDisable()
    {
        if (health != null)
            health.Damaged -= OnDamaged;
    }

    void OnDamaged(int currentHealth, int maxHealth)
    {
        if (healthBar != null && maxHealth > 0)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
