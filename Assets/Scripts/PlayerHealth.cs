using System;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public event Action<int, int> Damaged;
    public event Action<int, int> Healed;
    public event Action<int> ScoreChanged;
    public event Action Died;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    int current = 100;
    int max = 100;
    int score = 0;
    bool isDead = false;

    void Start()
    {
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        current = max;
        score = 0;
        isDead = false;
        UpdateHUDText();
        Healed?.Invoke(current, max);
    }

    public void CollectStar()
    {
        if (isDead) return;

        score += 100;
        UpdateHUDText();
        ScoreChanged?.Invoke(score);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPickup();
    }

    public void UseBoost()
    {
        if (isDead) return;

        score += 250;
        UpdateHUDText();
        ScoreChanged?.Invoke(score);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBoost();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        current -= amount;
        if (current < 0) current = 0;

        UpdateHUDText();
        Damaged?.Invoke(current, max);

        if (current == 0)
        {
            isDead = true;
            Died?.Invoke();
        }
    }

    public void TakeDamage25()
    {
        TakeDamage(25);
    }

    void UpdateHUDText()
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + score;

        if (livesText != null)
            livesText.text = "HP: " + current + "/" + max;
    }
}