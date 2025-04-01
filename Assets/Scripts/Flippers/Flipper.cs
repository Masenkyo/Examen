using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Flipper : MonoBehaviour
{
    // Rotation variables
    [SerializeField] int rotateSpeed = 45;
    [HideInInspector] public bool doubleSpeedPressed;
    [HideInInspector] public float DesiredHorizontalMovement;
    int doubleSpeed = 1;
    float rotation;
    Rigidbody2D rigidbody;
    
    // The list of all the flippers
    public static List<Flipper> AllFlippers = new();
    
    #region FlipperList
    
    // Adding the flippers to the list and giving the rigidbody variable a rigidbody of the flipper
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        AllFlippers.Add(this);
    }
    
    // Remove everything in the list becaues it's static
    void OnDestroy() => AllFlippers.Remove(this);

    #endregion

    void FixedUpdate() => InputRotations();

    void Update() => DoubleSpeed();

    // Activating this through pressing the correct button will make the flippers rotate twice as fast
    void DoubleSpeed() => doubleSpeed = Gamepad.current is { } ? doubleSpeedPressed ? 2 : 1 : 1;

    // The rotation system
    void InputRotations() => rigidbody.angularVelocity = DesiredHorizontalMovement < 0 
        ? rotateSpeed * doubleSpeed * -DesiredHorizontalMovement
        : DesiredHorizontalMovement > 0
            ? -rotateSpeed * doubleSpeed * DesiredHorizontalMovement
            : 0;
}
