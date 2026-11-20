using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIScreen startScreen;

    public static bool openHUDAfterLoading = false;

    UIScreen current;
    Stack<UIScreen> history = new Stack<UIScreen>();

    void Start()
    {
        UIScreen[] all = FindObjectsByType<UIScreen>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (UIScreen s in all)
            s.gameObject.SetActive(false);

        if (openHUDAfterLoading)
        {
            openHUDAfterLoading = false;

            foreach (UIScreen s in all)
            {
                if (s.gameObject.name == "Screen_HUD")
                {
                    current = s;
                    break;
                }
            }

            if (current == null)
                current = startScreen;
        }
        else
        {
            current = startScreen;
        }

        if (current != null)
            current.Show(true);
    }

    public void Open(UIScreen next)
    {
        if (current != null)
        {
            current.Hide(true);
            history.Push(current);
        }

        current = next;
        next.Show(true);
    }

    public void Back()
    {
        if (history.Count == 0) return;

        current.Hide(false);
        current = history.Pop();
        current.Show(false);
    }

    public void OpenAndClearHistory(UIScreen next)
    {
        history.Clear();

        if (current != null)
            current.Hide(false);

        current = next;
        current.Show(true);
    }
}