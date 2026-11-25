using UnityEngine;
using UnityEngine.InputSystem;

public class StarDashPlayer : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float xLimit = 2.4f;
    public float yLimit = 4.2f;

    public PlayerHealth playerHealth;
    public UIManager uiManager;
    public UIScreen victoryScreen;

    void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.Won += HandleWin;
    }

    void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.Won -= HandleWin;
    }

    void Update()
    {
        if (Time.timeScale == 0f || Keyboard.current == null) return;

        float h = 0f;
        float v = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v -= 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v += 1f;

        Vector3 pos = transform.position + new Vector3(h, v, 0f).normalized * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        pos.y = Mathf.Clamp(pos.y, -yLimit, yLimit);
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerHealth == null) return;

        if (other.CompareTag("Star"))
        {
            playerHealth.CollectStar();
            other.transform.position = new Vector3(Random.Range(-2.2f, 2.2f), 5.5f, 0f);
        }
        else if (other.CompareTag("Boost"))
        {
            playerHealth.UseBoost();
            other.transform.position = new Vector3(Random.Range(-2.2f, 2.2f), 6.5f, 0f);
        }
        else if (other.CompareTag("Hazard"))
        {
            playerHealth.TakeDamage25();
            other.transform.position = new Vector3(Random.Range(-2.2f, 2.2f), 6.0f, 0f);
        }
    }

    void HandleWin()
    {
        if (uiManager != null && victoryScreen != null)
            uiManager.OpenAndClearHistory(victoryScreen);
    }
}