using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    float startTime;
    void Start()
    {
        startTime = Time.time;
    }
    
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime / 3;

        if (Time.time - startTime > 1)
        {
            Text.color -= new Color(0, 0, 0, 1f * Time.deltaTime);
            if (Text.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public TMP_Text Text;
    public void SetText(string text, Color color)
    {
        Text.text = text;
        Text.color = color;
    }
}
