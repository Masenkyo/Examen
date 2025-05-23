using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepairPrompt : MonoBehaviour
{
    public Sprite Unpressed, Pressed;
    public SpriteRenderer SR;
    SpriteRenderer ownSr;

    Coroutine started;
    IEnumerator hide()
    {
        ownSr.enabled = false;
        SR.enabled = false;
        yield return new WaitForSeconds(2f);
        ownSr.enabled = true;
        SR.enabled = true;
    }
    
    void Update()
    {
        if (Gamepad.all.Any(_ => _.buttonSouth.wasPressedThisFrame) || Input.GetKeyDown(KeyCode.B))
        {
            if (started != null)
                StopCoroutine(started);
            started = StartCoroutine(hide());
        }
    }
    
    void Start()
    {
        ownSr = GetComponent<SpriteRenderer>();
        
        StartCoroutine(hide());
        StartCoroutine(loop());
        IEnumerator loop()
        {
            while (true)
            {
                SR.sprite = Unpressed;
                yield return new WaitForSeconds(0.5f);
                SR.sprite = Pressed;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
