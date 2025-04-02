using UnityEngine;
using UnityEngine.UI;

public class ProgressBarr : MonoBehaviour
{
    Image progressBar;
    [SerializeField]
    Image indicator;
    float begin;
    float end;
    public float position;
    float indicatorSize;

    void Awake()
    {
        progressBar = GetComponent<Image>();
        indicatorSize = progressBar.rectTransform.localScale.x / 3;
        indicator.transform.localScale = new Vector2(indicatorSize, indicatorSize);
        begin = progressBar.rectTransform.position.y + indicatorSize / 2;
        //indicator.transform.position = new Vector2(progressBar.rectTransform.position.x - indicatorSize / 2, begin);
    }

    void Update()
    {
        //return;
        float y = progressBar.rectTransform.position.y /*- indicatorSize / 2*/ + position;
        indicator.transform.position = new Vector2(progressBar.rectTransform.position.x - indicatorSize / 2, y);
    }
}
