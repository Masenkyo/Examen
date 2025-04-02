using System;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    public static Action OnWin = () => { };
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Ball>(out _))
            OnWin();
    }
}