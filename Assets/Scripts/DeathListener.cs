using UnityEngine;

public class DeathListener : MonoBehaviour
{
    public PlayerHealth health;
    public UIManager uiManager;
    public UIScreen gameOverScreen;

    void OnEnable()
    {
        if (health != null)
            health.Died += OnDeath;
    }

    void OnDisable()
    {
        if (health != null)
            health.Died -= OnDeath;
    }

    void OnDeath()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDeath();

        if (uiManager != null && gameOverScreen != null)
            uiManager.OpenAndClearHistory(gameOverScreen);
    }
}