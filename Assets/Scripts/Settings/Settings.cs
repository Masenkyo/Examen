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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enabledSettings != null && enabledSettings.IsPressed() && paused) paused = false;

        if (!paused) return;

        foreach (var g in Gamepad.all)
        {
            if(g.startButton.IsPressed())
            {
                enabledSettings = g;
                paused = true;

            }
        }

    }
}
