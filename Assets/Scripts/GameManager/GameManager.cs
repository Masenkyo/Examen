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
        LevelSysteem.Ready += instance =>
        {
            LobbyManager.PlayerRemovedBefore += _ => StartCoroutine(wait());
            LobbyManager.PlayerAddedAfter += _ => StartCoroutine(wait());
            IEnumerator wait()
            {
                DistributeFlippers();
                Time.timeScale = 0;
                yield return new WaitForSecondsRealtime(3);
                Time.timeScale = 1;
            }
            
            DistributeFlippers();

            
            var rc = InputSystem.actions.FindActionMap("Main").FindAction("Rotate");
            rc.Enable();
        };
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
            var pointer = FlippersPointers[flipper];

            var diff = (edge - flipper.transform.position.y);
            if (diff < 1)
                diff = 1;
            float am = (1 / diff * Mathf.Pow(1, 1 + diff / 3));
            if (flipper.transform.position.y > edge || am < 0.25f)
                am = 0;
            pointer.rectTransform.localScale = new(am, am, 1);
            pointer.rectTransform.position = new Vector3(Camera.main.WorldToScreenPoint(new (flipper.transform.position.x, 0)).x, 2.5f + (40 * am));
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
        
        // 'incoming flipper' pointers
        foreach (var kvp in FlippersPointers)
            Destroy(kvp.Value.gameObject);
        FlippersPointers.Clear();
        foreach (var kvp in PlayersFlippers)
        {
            foreach (var flipper in kvp.Value)
            {
                var img = GameObject.Instantiate(pointerPrefab, transform.GetChild(0)).GetComponent<Image>();
                img.color = kvp.Key.ChosenPlayerEntrySet.color;
                FlippersPointers.Add(flipper, img);
            }
        }
    }
}