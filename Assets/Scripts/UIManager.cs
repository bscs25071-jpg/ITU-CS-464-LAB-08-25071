using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIScreen startScreen;

    UIScreen current;
    Stack<UIScreen> history = new Stack<UIScreen>();

    void Start()
    {
        UIScreen[] all = FindObjectsByType<UIScreen>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (UIScreen s in all)
            s.gameObject.SetActive(false);

        current = startScreen;
        if (current != null) current.Show(true);
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
        if (current != null) current.Hide(false);
        current = next;
        next.Show(true);
    }
}
