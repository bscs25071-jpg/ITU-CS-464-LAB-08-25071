using UnityEngine;

public class StarDashItem : MonoBehaviour
{
    public float fallSpeed = 2.5f;
    public float rotateSpeed = 90f;
    public float resetY = 5.2f;
    public float bottomY = -5.2f;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);

        if (fallSpeed > 0f)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            if (transform.position.y < bottomY)
            {
                float randomX = Random.Range(-2.2f, 2.2f);
                transform.position = new Vector3(randomX, resetY, 0f);
            }
        }
    }
}