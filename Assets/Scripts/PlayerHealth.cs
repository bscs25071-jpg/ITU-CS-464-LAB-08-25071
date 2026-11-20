using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<int, int> Damaged;
    public event Action Died;

    int current = 100;
    int max = 100;

    public void TakeDamage(int amount)
    {
        current -= amount;

        Damaged?.Invoke(current, max);

        if (current <= 0)
        {
            Died?.Invoke();
        }
    }
}