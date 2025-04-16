using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pointerPrefab;
    [SerializeField] Image resetFill;
    [SerializeField] GameObject youWin;
    
    public List<PlayerManager.Player> playersHoldingReset = new();

    [SerializeField] float holdDurationForReset = 2;
    
    class empty : MonoBehaviour { }
    
    void Awake()
    {
        StartCoroutine(resetInput());
        IEnumerator resetInput()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                foreach (var player in PlayerManager.Players)
                {
                    if (!player.Gamepad.buttonNorth.wasPressedThisFrame)
                        continue;
                    
                    playersHoldingReset.Add(player);
                    StartCoroutine(hold());
                    IEnumerator hold()
                    {
                        while (!player.Gamepad.buttonNorth.wasReleasedThisFrame)
                            yield return new WaitForEndOfFrame();
                        playersHoldingReset.Remove(player);
                    }
                }
            }
        }

        if (LevelSystem.instance != null && LevelSystem.instance.didStart)
            init();
        else
            LevelSystem.Ready = _ => init();

        void init()
        {
            Time.timeScale = 1;
            
            PlayerManager.PlayerRemovedBefore += e;
            PlayerManager.PlayerAddedAfter += e;

            GameStateClass.OnGameStateChanged += temp;
            void temp(GameStates gameStates)
            {
                GameStateClass.OnGameStateChanged -= temp;
                PlayerManager.PlayerRemovedBefore -= e;
                PlayerManager.PlayerAddedAfter -= e;
            }
            
            void e(PlayerManager.Player p)
            {
                var go = new GameObject("");
                go.AddComponent<empty>().StartCoroutine(wait());
                Destroy(go, 5);
                
                IEnumerator wait()
                {
                    DistributeFlippers();
                    Time.timeScale = 0;
                    yield return new WaitForSecondsRealtime(3);
                    Time.timeScale = 1;
                }
            }
            
            DistributeFlippers();
            InputSystem.actions.FindActionMap("Main").FindAction("Rotate").Enable();
            
            
            // What happens when everyone leaves
            PlayerManager.PlayerRemovedBefore += OnNoPlayer;
            GameStateClass.OnGameStateChanged += OnGameEnd;
            void OnNoPlayer(PlayerManager.Player p)
            {
                if (PlayerManager.Players.Count > 1)
                    return;
                PlayerManager.PlayerRemovedBefore -= OnNoPlayer;

                // End game
                GameStateClass.GameState = GameStates.InLobby;
            }
            void OnGameEnd(GameStates gameStates)
            {
                PlayerManager.PlayerRemovedBefore -= OnNoPlayer;
                GameStateClass.OnGameStateChanged -= OnGameEnd;
            }


            // What happens on win?
            WinArea.OnWin = () =>
            {
                StartCoroutine(wait());
                IEnumerator wait()
                {
                    Time.timeScale = 0;
                    youWin.SetActive(true);
                    yield return new WaitForSecondsRealtime(3);
                    youWin.SetActive(false);
                    Time.timeScale = 1;
                    GameStateClass.GameState = GameStates.InLobby;
                }
            };
        }
    }
    
    void Update()
    {
        // Resetting ball
        if (playersHoldingReset.Count == PlayerManager.Players.Count)
        {
            resetFill.fillAmount += 1f / holdDurationForReset * Time.deltaTime;

            if (resetFill.fillAmount >= 1f)
            {
                resetFill.fillAmount = 0;
                if (FindAnyObjectByType<Ball>() is { } ball)
                {
                    ball.Disable?.Invoke();
                    ball.Enable?.Invoke();
                }
            }
        }
        else
            resetFill.fillAmount = 0;
        
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
                var img = Instantiate(pointerPrefab, transform.GetChild(0)).GetComponent<Image>();
                img.color = kvp.Key.ChosenPlayerEntrySet.color;
                FlippersPointers.Add(flipper, img);
                flipper.GetComponent<SpriteRenderer>().color = kvp.Key.ChosenPlayerEntrySet.color;
            }
        }
    }
}