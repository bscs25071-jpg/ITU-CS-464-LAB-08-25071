
using UnityEngine;

public class DeathListener : MonoBehaviour
{
    public PlayerHealth health;
    public GameObject gameOverScreen;

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
        Debug.Log("Player Died! Game Over.");

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        Time.timeScale = 0f;
    }
}
