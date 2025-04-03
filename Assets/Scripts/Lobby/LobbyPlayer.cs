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
    
    public Color Color
    {
        get => _color;
        set
        {
            if (id is { })
                id.color = value;
            if (avatar is { })
                avatar.color = value;
            _color = value;
        }
    } [SerializeField, HideInInspector] Color _color;

    void Awake()
    {
        Color = Color;
    }
}