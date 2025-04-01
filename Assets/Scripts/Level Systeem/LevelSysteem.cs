using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LevelSysteem : MonoBehaviour
{
    public static LevelSysteem instance;
    [SerializeField] List<Transform> Levels = new();
    [SerializeField] int amountOfLevels = 10;
    float gapBetweenLevels = -8.5f;
    public static Action<LevelSysteem> Ready = _ => { };

    void Start() => InstantiateLevels();
    
    void Awake() => instance = this;
    void InstantiateLevels()
    {
        for (float i = 0; i > amountOfLevels * gapBetweenLevels; i += gapBetweenLevels) 
            Instantiate(Levels[UnityEngine.Random.Range(0, Levels.Count)], new Vector3(0, i, 0), Quaternion.identity);

        Ready(this);
    }
}
