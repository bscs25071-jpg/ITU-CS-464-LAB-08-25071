using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Image loadingBar;
    public TMP_Text loadingText;

    void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }

    IEnumerator LoadSceneProgress()
    {
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime / 3f;

            loadingBar.fillAmount = progress;

            loadingText.text = "Loading... " + (int)(progress * 100) + "%";

            yield return null;
        }

        loadingBar.fillAmount = 1f;
        loadingText.text = "Loading... 100%";
        UIManager.openHUDAfterLoading = true;
        SceneManager.LoadScene("SampleScene");
    }
}