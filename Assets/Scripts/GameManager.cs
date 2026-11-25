using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameplayWorld;
    public GameObject hudScreen;
    public GameObject menuBackground;

    public void StartGame()
    {
        SceneManager.LoadScene("Loading");
    }

    void Update()
    {
        if (gameplayWorld != null && hudScreen != null)
        {
            bool isPlaying = hudScreen.activeSelf;
            gameplayWorld.SetActive(isPlaying);

            if (menuBackground != null)
                menuBackground.SetActive(!isPlaying);
        }
    }
}