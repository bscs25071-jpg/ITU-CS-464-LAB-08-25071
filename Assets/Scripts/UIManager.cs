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
        {
            current.Show(true);
            HandleScreenAudio(current.gameObject.name, true);
        }
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

        if (AudioManager.Instance != null)
        {
            if (next.gameObject.name == "Screen_Pause")
                AudioManager.Instance.PlayPause();
            else
                AudioManager.Instance.PlayPanelOpen();
        }

        HandleScreenAudio(next.gameObject.name, true);
    }

    public void Back()
    {
        if (history.Count == 0) return;

        string closingName = current.gameObject.name;

        current.Hide(false);
        current = history.Pop();
        current.Show(false);

        if (AudioManager.Instance != null)
        {
            if (closingName == "Screen_Pause")
                AudioManager.Instance.PlayResume();
            else
                AudioManager.Instance.PlayPanelClose();
        }

        HandleScreenAudio(current.gameObject.name, false);
    }

    public void OpenAndClearHistory(UIScreen next)
    {
        history.Clear();

        if (current != null)
            current.Hide(false);

        current = next;
        current.Show(true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPanelOpen();

        HandleScreenAudio(next.gameObject.name, true);
    }

    void HandleScreenAudio(string screenName, bool isOpening)
    {
        if (AudioManager.Instance == null) return;

        if (screenName == "Screen_HUD")
        {
            AudioManager.Instance.PlayGameplayMusic();
        }
        else if (screenName == "Screen_MainMenu")
        {
            AudioManager.Instance.PlayMenuMusic();
        }
        else if (screenName == "Screen_Victory")
        {
            AudioManager.Instance.PlayResultMusic();
            if (isOpening) AudioManager.Instance.PlayWin();
        }
        else if (screenName == "Screen_GameOver")
        {
            AudioManager.Instance.PlayResultMusic();
            if (isOpening) AudioManager.Instance.PlayLose();
        }
    }
}