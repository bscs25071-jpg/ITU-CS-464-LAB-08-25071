
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<int, int> Damaged;
    public event Action Died;

    int current = 100;
    int max = 100;
    bool isDead = false;

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        current -= amount;

        if (current < 0)
            current = 0;

        Damaged?.Invoke(current, max);

        if (current == 0)
        {
            isDead = true;
            Died?.Invoke();
        }
    }
}
