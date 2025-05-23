using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public static bool tutorialActive = false;
    void Start()
    {
        tutorialActive = true;
        
        Time.timeScale = 0;
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(25);
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync("Tutorial");
        }
    }

    void OnDestroy()
    {
        tutorialActive = false;
    }

    public void SkipButton()
    {
        StopAllCoroutines();
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("Tutorial");
    }
}