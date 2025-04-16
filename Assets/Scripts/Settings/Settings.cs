using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    Button continuteButton;
    [SerializeField]
    Button settingsButton;
    [SerializeField]
    Button quitButton;
    [SerializeField]
    AudioSource gameplayAudio;
    [SerializeField]
    GameObject settingsMenu;
    Slider volumeSlider;
    Button returnFromSettings;
    
    bool paused = false;
    bool pressed = false;
    Gamepad enabledSettings = null;
    float originalVolume;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        originalVolume = gameplayAudio.volume;
        volumeSlider = settingsMenu.GetComponentInChildren<Slider>();
        returnFromSettings = settingsMenu.GetComponentInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var g in Gamepad.all)
        {
            if (g.startButton.isPressed)
            {
                enabledSettings = g;
            }
        }

        if (enabledSettings == null) return;
        if (enabledSettings.startButton.isPressed != pressed)
        {
            pressed = enabledSettings.startButton.isPressed;
            if(enabledSettings.startButton.isPressed)
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        continuteButton.gameObject.SetActive(paused);
        settingsButton.gameObject.SetActive(paused);
        quitButton.gameObject.SetActive(paused);
        if(paused)
        {
            returnFromSettings.gameObject.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
