using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flippers : MonoBehaviour
{
    [SerializeField]
    int rotateSpeed = 45;
    float rotation;
    
    static List<Transform> flippers = new();
    
    #region FlipperList
    
    void Awake() => flippers.Add(transform);
    
    void OnDestroy() => flippers.Remove(transform);
    
    #endregion

    void Update() => InputRotations();
    
    void InputRotations() => flippers.ForEach(_ => _.GetComponent<Rigidbody2D>().angularVelocity = Input.GetKey(KeyCode.A) 
        ? rotateSpeed
        : Input.GetKey(KeyCode.D) 
            ? -rotateSpeed
            : 0);
}
