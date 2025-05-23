using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct BarSprites
{
    public ItemName name;
    public Sprite indicator;
    public Sprite bar;
}

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar reference;
    Image progressBar;
    [SerializeField]
    Image indicator;
    [SerializeField]
    Transform levelBegin;
    [SerializeField]
    Transform ball;
    Vector3 levelEnd;
    float begin;
    float end;
    public float position;
    float indicatorSize;
    public float indicatorScale = 0.3f;

    [SerializeField] BarSprites[] allBarSprites = new BarSprites[9];

    public void SetBar(ItemName name)
    {
        foreach (var barsprite in allBarSprites)
        {
            if (name == barsprite.name)
            {
                indicator.sprite = barsprite.indicator;
                progressBar.sprite = barsprite.bar;
                break;
            }
        }

    }

    void GetEnd()
    {
        float end = LevelSystem.instance.amountOfLevels * LevelSystem.instance.gapBetweenLevels;
        levelEnd = new Vector3(0, end);
    }

    void Awake()
    {
        reference = this;
        progressBar = GetComponent<Image>();
        indicatorSize = progressBar.rectTransform.localScale.x * indicatorScale;
        begin = progressBar.rectTransform.position.y + (progressBar.rectTransform.rect.height / 2);
        end = progressBar.rectTransform.position.y - (progressBar.rectTransform.rect.height / 2);
        indicator.transform.localScale = new Vector2(indicatorSize, indicatorSize);
        indicator.rectTransform.position = new Vector2(indicator.rectTransform.position.x, end);
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
        float convertedRange = ConvertRange(ball.position.y, levelBegin.position.y, levelEnd.y, begin, end);
        position = Clamp(convertedRange, begin + 0.1f, end - 0.1f);
        indicator.rectTransform.position = new Vector2(indicator.rectTransform.position.x, position);
    }
}

