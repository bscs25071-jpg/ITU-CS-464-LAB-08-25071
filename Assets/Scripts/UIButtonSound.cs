using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    public bool isBackButton = false;

    Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        string lower = gameObject.name.ToLower();
        if (lower.Contains("back") || lower.Contains("close") || lower.Contains("cancel") || lower.Contains("quit"))
            isBackButton = true;
    }

    void OnEnable()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(PlayClick);
    }

    void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(PlayClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && button.interactable && AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonHover();
    }

    void PlayClick()
    {
        if (AudioManager.Instance == null) return;

        if (isBackButton)
            AudioManager.Instance.PlayButtonBack();
        else
            AudioManager.Instance.PlayButtonClick();
    }
}