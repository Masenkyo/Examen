using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    Image progressBar;
    [SerializeField]
    Image indicator;
    [SerializeField]
    Transform levelBegin;
    [SerializeField]
    Transform ball;
    [SerializeField]
    Transform levelEnd;
    float begin;
    float end;
    public float position;
    float indicatorSize;
    public float indicatorScale = 0.3f;

    void GetEnd()
    {
        float end = LevelSystem.instance.amountOfLevels * LevelSystem.instance.gapBetweenLevels;
        var temp = new Transform();
        temp.position = new Vector2(0, end);
        levelEnd = temp.position;
    }

    void Awake()
    {
        progressBar = GetComponent<Image>();
        indicatorSize = progressBar.rectTransform.localScale.x * indicatorScale;
        begin = progressBar.rectTransform.position.y + (progressBar.rectTransform.rect.height / 2);
        end = progressBar.rectTransform.position.y - (progressBar.rectTransform.rect.height / 2);
        indicator.transform.localScale = new Vector2(indicatorSize, indicatorSize);
        indicator.rectTransform.position = new Vector2(indicator.rectTransform.position.x - progressBar.rectTransform.rect.width / 2, end);
    }
    
    void Start() => GetEnd();

    float Clamp(float value, float min, float max)
    {
        if (min > max)
        {
            float temp = min;
            min = max;
            max = temp;
        }
        if (value < min) return min;
        if (value > max) return max;

        return value;
    }



    float ConvertRange(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        return (((oldValue - oldMin) * newRange) / oldRange) + newMin;
    }

    void Update()
    {
        float convertedRange = ConvertRange(ball.position.y, levelBegin.position.y, levelEnd.position.y, begin, end);
        position = Clamp(convertedRange, begin + 0.1f, end - 0.1f);
        indicator.rectTransform.position = new Vector2(indicator.rectTransform.position.x, position);
    }
}

