using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] GameObject crown;
    public TMP_Text id;
    public Image avatar;
    
    public bool IsKing
    {
        get => _isKing;
        set
        {
            if (crown != null)
                crown.SetActive(value);
            _isKing = value;
        }
    } bool _isKing;

    public Image triangle;
    public Sprite Unpressed, Pressed;
    
    public bool TrianglePressed
    {
        get => _trianglePressed;
        set
        {
            _trianglePressed = value;
            if (triangle == null) return;
            
            if (startedDelayedPress != null)
                StopCoroutine(startedDelayedPress);
            
            if (value && triangle.color.a <= 0f)
            {
                startedDelayedPress = StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(0.1f);
                    triangle.sprite = Pressed;
                }
            }
            else
                triangle.sprite = value ? Pressed : Unpressed;

            if (startedFadeOut != null)
                StopCoroutine(startedFadeOut);
            
            if (value)
                triangle.color = new Color(triangle.color.r, triangle.color.g, triangle.color.b,1);
            else
                startedFadeOut = StartCoroutine(fadeOut());
        }
    } bool _trianglePressed;

    Coroutine startedDelayedPress;
    Coroutine startedFadeOut;
    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForFixedUpdate();
            triangle.color -= new Color(0,0,0,2f * 1/60f);
        }
    }
    
    public Color Color
    {
        get => _color;
        set
        {
            if (id is { })
                id.color = value;
            if (avatar is { })
                avatar.color = value;
            if (triangle is { })
            {
                triangle.color = value;
                triangle.color -= new Color(0, 0, 0, 1);
            }
            _color = value;
        }
    } [SerializeField, HideInInspector] Color _color;

    void Awake()
    {
        Color = Color;
    }
}