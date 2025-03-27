using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pointerPrefab;
    
    void Awake()
    {
        LobbyManager.PlayerRemovedBefore += _ =>
        {
            DistributeFlippers();
            StartCoroutine(wait());
        };
        LobbyManager.PlayerAddedAfter += _ =>
        {
            DistributeFlippers();
            StartCoroutine(wait());
        };

        IEnumerator wait()
        {
            Time.timeScale = 0;
            yield return new WaitForSeconds(3);
            Time.timeScale = 1;
        }
        
        DistributeFlippers();

        FlippersPointers = PlayersFlippers.SelectMany(_ => _.Value).ToDictionary(_ => _, _ => GameObject.Instantiate(pointerPrefab, pointerPrefab.transform.parent).GetComponent<Image>());
        
        var rc = InputSystem.actions.FindActionMap("Main").FindAction("Rotate");
        rc.Enable();
    }

    void Update()
    {
        foreach (var keyValuePair in PlayersFlippers)
            keyValuePair.Value.ForEach(_ =>
            {
                _.doubleSpeedPressed = keyValuePair.Key.Gamepad.squareButton.isPressed;
                _.DesiredHorizontalMovement = keyValuePair.Key.Gamepad.leftStick.value.x;
            });
        
        // Distance pointers
        float edge = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        foreach (var flipper in PlayersFlippers.SelectMany(_ => _.Value))
        {
            if (flipper.transform.position.y < edge)
            {
                var pointer = FlippersPointers[flipper];
                pointer.transform.position = new (flipper.transform.position.x, 10);
                float am = (edge - flipper.transform.position.y)
                    switch
                    {
                        < -0 => 0,
                        < 2  => 1f,
                        < 4  => 0.5f,
                        < 6  => 0.25f,
                        < 8  => 0.1275f,
                        < 16 => 0.06375f,
                    };
                pointer.transform.localScale = new(am, am, 1);
            }
        }
    }

    readonly Dictionary<PlayerManager.Player, List<Flipper>> PlayersFlippers = new();
    Dictionary<Flipper, Image> FlippersPointers = new();
    
    void DistributeFlippers()
    {
        PlayersFlippers.Clear();
        var playerBuffer = PlayerManager.Players.ToList();
        if (playerBuffer.Count == 0)
            return;
        foreach (var flipper in Flipper.AllFlippers)
        {
            if (!PlayersFlippers.ContainsKey(playerBuffer.First()))
                PlayersFlippers.Add(playerBuffer.First(), new());
                
            PlayersFlippers[playerBuffer.First()].Add(flipper);
            playerBuffer.RemoveAt(0);
            if (playerBuffer.Count == 0)
                playerBuffer = PlayerManager.Players.ToList();
        }
    }
}