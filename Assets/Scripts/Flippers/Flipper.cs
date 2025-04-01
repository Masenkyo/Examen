using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Flipper : MonoBehaviour
{
    [SerializeField] int rotateSpeed = 45;
    [HideInInspector] public bool doubleSpeedPressed;
    [HideInInspector] public float DesiredHorizontalMovement;
    int doubleSpeed = 1;
    float rotation;
    Rigidbody2D rigidbody;
    
    public static List<Flipper> AllFlippers = new();
    
    #region FlipperList
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        AllFlippers.Add(this);
    }
    
    void OnDestroy() => AllFlippers.Remove(this);

    #endregion

    void FixedUpdate() => InputRotations();

    void Update() => DoubleSpeed();

    void DoubleSpeed() => doubleSpeed = Gamepad.current is { } ? doubleSpeedPressed ? 2 : 1 : 1;

    void InputRotations() => rigidbody.angularVelocity = DesiredHorizontalMovement < 0 
        ? rotateSpeed * doubleSpeed * -DesiredHorizontalMovement
        : DesiredHorizontalMovement > 0
            ? -rotateSpeed * doubleSpeed * DesiredHorizontalMovement
            : 0;
}
