using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    // Level settings and list
    [SerializeField] List<Transform> Levels = new();
    public int amountOfLevels = 10;
    public float gapBetweenLevels = -8.5f;
    public static Action<LevelSystem> Ready = _ => { };

    // A reference to this script
    public static LevelSystem instance;
    
    void Start() => InstantiateLevels();
    
    // The reference
    void Awake() => instance = this;

    // Instantiate random levels on specified places
    void InstantiateLevels()
    {
        for (float i = 0; i > amountOfLevels * gapBetweenLevels; i += gapBetweenLevels) 
            Instantiate(Levels[UnityEngine.Random.Range(0, Levels.Count)], new Vector3(0, i, 0), Quaternion.identity);

        Ready(this);
    }
}