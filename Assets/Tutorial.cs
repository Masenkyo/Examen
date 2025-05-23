using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public static bool tutorialActive = false;
    void Awake()
    {
        Time.timeScale = 0;
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSecondsRealtime(25);
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync("Tutorial");
        }
    }

    void OnDestroy()
    {
        tutorialActive = false;
    }

    void Update()
    {
        if ((!Gamepad.current?.buttonSouth.wasPressedThisFrame ?? true) && !Input.GetKeyDown(KeyCode.B))
            return;

        enabled = false;
        StopAllCoroutines();
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("Tutorial");
    }
}