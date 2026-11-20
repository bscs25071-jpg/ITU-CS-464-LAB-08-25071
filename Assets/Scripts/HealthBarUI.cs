using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth health;
    public Image healthBar;

    void OnEnable()
    {
        health.Damaged += OnDamaged;
    }

    void OnDisable()
    {
        health.Damaged -= OnDamaged;
    }

    void OnDamaged(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
