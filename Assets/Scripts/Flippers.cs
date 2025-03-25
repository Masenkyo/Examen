using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flippers : MonoBehaviour
{
    [SerializeField]
    int rotateSpeed = 45;
    int doubleSpeed = 1;
    float rotation;
    
    static List<Transform> flippers = new();
    
    #region FlipperList
    
    void Awake() => flippers.Add(transform);
    
    void OnDestroy() => flippers.Remove(transform);
    
    #endregion

    void FixedUpdate() => InputRotations();

    void Update() => DoubleSpeed();

    void DoubleSpeed() => doubleSpeed = Gamepad.current.buttonWest.isPressed ? 2 : 1;
    
    void InputRotations() => flippers.ForEach(_ => _.GetComponent<Rigidbody2D>().angularVelocity = Input.GetAxis("Horizontal") < 0 
        ? rotateSpeed * doubleSpeed * -Input.GetAxis("Horizontal")
        : Input.GetAxis("Horizontal") > 0
            ? -rotateSpeed * doubleSpeed * Input.GetAxis("Horizontal")
            : 0);
}
