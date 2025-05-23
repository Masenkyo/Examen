using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            Image.color -= new Color(0, 0, 0, 1f * Time.deltaTime);
            if (Text.color.a <= 0)
                Destroy(gameObject);
        }
    }

    [SerializeField] TMP_Text Text;
    [SerializeField] Image Image;
    
    public void SetText(string text, Color color, Sprite sprite)
    {
        Text.text = text;
        Text.color = color;
        Image.sprite = sprite;
    }
}
