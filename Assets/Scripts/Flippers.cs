using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = System.Numerics.Vector3;

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

    void Update()
    {
        InputRotations();
        LaunchFlipper();
        DoubleSpeed();
    }
    
    void DoubleSpeed() => doubleSpeed = Gamepad.current is { } ? Gamepad.current.buttonWest.isPressed ? 2 : 1 : 1;
    
    void InputRotations() => flippers.Where(_ => _.CompareTag("Flipper"))
        .ToList()
        .ForEach(_ => _.GetComponent<Rigidbody2D>().angularVelocity = Input.GetAxis("Horizontal") < 0 
        ? rotateSpeed * doubleSpeed * -Input.GetAxis("Horizontal")
        : Input.GetAxis("Horizontal") > 0
            ? -rotateSpeed * doubleSpeed * Input.GetAxis("Horizontal")
            : 0);

    void LaunchFlipper() => flippers
        .Where(_ => _.CompareTag("LaunchFlipper"))
        .ToList()
        .ForEach(_ => _.GetComponent<Rigidbody2D>().rotation = Input.GetAxis("Vertical") * 90);
    // .SetRotation(Input.GetAxis("Vertical") * 90));

    void hi()
    {
        Vector3.Lerp(new Vector3(0), new Vector3(90f), Input.GetAxis("Vertical"));
    }
}
