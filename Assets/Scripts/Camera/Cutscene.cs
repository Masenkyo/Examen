using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour
{
    // Event for if you want to spawn the ball before the follow script gets activated to prevent errors
    [SerializeField] UnityEvent spawnBall;
    
    // The Cutscene itself | Public variable for if you want to use it in a different script
    public Action startCutscene => transform.position.y < 0
        ? () => transform.position += new Vector3(0, height * Time.deltaTime * speedMultiplier, 0)
        : () =>
        {
            spawnBall.Invoke();
            Follow.reference.enabled = true;
            enabled = false;
        };
    
    // CameraMovespeed variables
    [SerializeField] int speedMultiplier = 2;
    float height = 7.5f;
    
    // Put the camera at the correct height automatically using the amount of levels and how big the gaps between levels are
    void Awake()
    {
        LevelSystem levelSystem = FindObjectsByType<LevelSystem>(FindObjectsSortMode.None).FirstOrDefault();
        transform.position = new Vector3(transform.position.x, levelSystem.amountOfLevels * levelSystem.gapBetweenLevels, transform.position.z);
    }

    // Calling the Cutscene | Remove update and call CutsceneOfLevel in different script if you want to change when the cutscene begins
    void Update() => startCutscene();
}
