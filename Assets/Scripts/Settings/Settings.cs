using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    Button continuteButton;
    [SerializeField]
    Button settingsButton;
    [SerializeField]
    Button quitButton;

    bool paused = false;

    Gamepad enabledSettings = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        continuteButton.gameObject.SetActive(paused);
        settingsButton.gameObject.SetActive(paused);
        quitButton.gameObject.SetActive(paused);
        
        if (enabledSettings != null && enabledSettings.IsPressed() && paused) paused = false;

        foreach (var g in Gamepad.all)
        {
            if (paused) break;
            if(g.startButton.IsPressed())
            {
                Debug.Log("dajsdklasd");
                enabledSettings = g;
                paused = true;

            }
        }
    }
}
