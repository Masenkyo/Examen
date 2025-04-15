using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Flipper : MonoBehaviour
{
    // Broken Flipper variables
    public bool brokenFlipper;
    float time = 0;
    bool doOnce;
    
    // Rotation variables
    [SerializeField] int rotateSpeed = 45;
    [HideInInspector] public bool doubleSpeedPressed;
    [HideInInspector] public float DesiredHorizontalMovement;
    int doubleSpeed = 1;
    float rotation;
    
    [HideInInspector] public Rigidbody2D rigidbody;
    
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

    void Update()
    {
        DoubleSpeed();
        ActiveFlippers();
        
        if (rigidbody.simulated && Follow.reference.enabled && !doOnce)
            BrokenFlipper(max: 3); 
            
        if (brokenFlipper)
            FixBrokenFlipper();
    } 

    // Activating this through pressing the correct button will make the flippers rotate twice as fast
    void DoubleSpeed() => doubleSpeed = Gamepad.current is { } ? doubleSpeedPressed ? 2 : 1 : 1;

    // The rotation system
    void InputRotations() => rigidbody.angularVelocity = DesiredHorizontalMovement < 0 && !brokenFlipper
        ? rotateSpeed * doubleSpeed * -DesiredHorizontalMovement
        : DesiredHorizontalMovement > 0 && !brokenFlipper
            ? -rotateSpeed * doubleSpeed * DesiredHorizontalMovement
            : 0;
    
    void BrokenFlipper(int min = 0, int max = 20)
    {
        if (UnityEngine.Random.Range(min, max) != 0)
        {
            doOnce = true;
            return;
        }
    
        brokenFlipper = true;
        doOnce = true;
    }
    
    void ActiveFlippers()
    {
        float highLimit = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y + 1;
        float lowLimit = -((highLimit -1) / 3);
        
        rigidbody.simulated = transform.position.y >= lowLimit && transform.position.y <= highLimit;
        
        if (transform.position.y > highLimit || transform.position.y < lowLimit)
        {
            brokenFlipper = false;
            doOnce = false;
        }
        
        if (!rigidbody.simulated || brokenFlipper)
            transform.rotation = Quaternion.Euler(0, 0, !name.Contains("Oval") ? 0 : 90);
    }
    
    void FixBrokenFlipper()
    {
        if (time > 0)
            time -= Time.deltaTime; // * PlayerManager.Players.Count;
        
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            time += 1;
    
        if (time > 4)
        {
            brokenFlipper = false;
            time = 0;
            doOnce = false;
        }
    }
}
