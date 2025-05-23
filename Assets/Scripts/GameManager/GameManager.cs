using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pointerPrefab;
    [SerializeField] Image resetFill;
    [SerializeField] GameObject youWin;
    
    public readonly List<PlayerManager.Player> playersHoldingReset = new();

    [SerializeField] float holdDurationForReset = 2;
    
    class empty : MonoBehaviour { }
    
    void Awake()
    {
        LobbyManager.map[(LobbyManager.inputs.Reset, 0)].OnDown(() =>
        {
            playersHoldingReset.Add(PlayerManager.Players[0]);
            PlayerManager.Players[0].LP.TrianglePressed = true;
        });
        LobbyManager.map[(LobbyManager.inputs.Reset, 0)].OnUp(() =>
        {
            playersHoldingReset.Remove(PlayerManager.Players[0]);
            PlayerManager.Players[0].LP.TrianglePressed = false;
        });
        LobbyManager.map[(LobbyManager.inputs.Reset, 1)].OnDown(() =>
        {
            playersHoldingReset.Add(PlayerManager.Players[1]);
            PlayerManager.Players[1].LP.TrianglePressed = true;
        });
        LobbyManager.map[(LobbyManager.inputs.Reset, 1)].OnUp(() =>
        {
            playersHoldingReset.Remove(PlayerManager.Players[1]);
            PlayerManager.Players[1].LP.TrianglePressed = false;
        });
        
        StartCoroutine(resetInput());
        IEnumerator resetInput()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                foreach (var player in PlayerManager.Players)
                {
                    if (player.Gamepad == null)
                        continue;
                    if (!player.Gamepad.buttonNorth.wasPressedThisFrame)
                        continue;

                    playersHoldingReset.Add(player);
                    player.LP.TrianglePressed = true;
                    StartCoroutine(hold());
                    IEnumerator hold()
                    {
                        while (!player.Gamepad.buttonNorth.wasReleasedThisFrame)
                            yield return new WaitForEndOfFrame();
                        playersHoldingReset.Remove(player);
                        player.LP.TrianglePressed = false;
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
            Tutorial.tutorialActive = true;
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);
            
            Time.timeScale = 1;
            
            PlayerManager.PlayerRemovedAfter += WaitAndDistribute;
            PlayerManager.PlayerAddedAfter += WaitAndDistributeButWithUnusedParameter;

            GameStateClass.OnGameStateChanged += temp;
            void temp(GameStates gameStates)
            {
                GameStateClass.OnGameStateChanged -= temp;
                PlayerManager.PlayerRemovedAfter -= WaitAndDistribute;
                PlayerManager.PlayerAddedAfter -= WaitAndDistributeButWithUnusedParameter;
            }
            
            void WaitAndDistributeButWithUnusedParameter(PlayerManager.Player p) => WaitAndDistribute();
            void WaitAndDistribute()
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
        {
            if (keyValuePair.Key.Gamepad is { } gp)
                keyValuePair.Value.ForEach(_ =>
                {
                    if (_.accessTypes.Any(_ => _ is ControlAccessTypes.Rotation))
                    {
                        _.flipper.DesiredHorizontalMovement = gp.leftStick.value.x;
                    }
                    
                    if (_.accessTypes.Any(_ => _ is ControlAccessTypes.Movement))
                    {
                        (_.flipper as MovingFlipper).InputJoystickMovement = gp.leftStick.value;
                    }
                });
            else
                keyValuePair.Value.ForEach(_ =>
                {
                    if (_.accessTypes.Any(_ => _ is ControlAccessTypes.Rotation))
                    {
                        float d = 0;
                        if (Input.GetKey(
                                LobbyManager.map[(LobbyManager.inputs.Left, (int)keyValuePair.Key.KeyboardID)]))
                            d = -1;
                        if (Input.GetKey(
                                LobbyManager.map[(LobbyManager.inputs.Right, (int)keyValuePair.Key.KeyboardID)]))
                            d = 1;

                        _.flipper.DesiredHorizontalMovement = d;
                    }
                    
                    if (_.accessTypes.Any(_ => _ is ControlAccessTypes.Movement))
                    {//
                        float d = 0;
                        if (Input.GetKey(
                                LobbyManager.map[(LobbyManager.inputs.Left, (int)keyValuePair.Key.KeyboardID)]))
                            d = -1;
                        if (Input.GetKey(
                                LobbyManager.map[(LobbyManager.inputs.Right, (int)keyValuePair.Key.KeyboardID)]))
                            d = 1;
                    }
                });
        }
        
        // Distance pointers
        float edge = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        foreach (var flipper in PlayersFlippers.SelectMany(_ => _.Value).Select(_ => _.flipper))
        {
            var pointer = FlippersPointers[flipper];

            var diff = (edge - flipper.transform.position.y);
            diff /= 2;
            if (diff < 1)
                diff = 1;
            float am = (1 / diff * Mathf.Pow(1, 1 + diff / 3));
            if (flipper.transform.position.y > edge || am < 0.25f)
                am = 0;
            pointer.transform.localScale = new(am, am, 1);
            pointer.transform.position = new Vector3(Camera.main.WorldToScreenPoint(new (flipper.transform.position.x, 0)).x, 2.5f + (40 * am));
        }
    }

    enum ControlAccessTypes { Movement, Rotation }
    struct ControlFragment
    {
        public Flipper flipper;
        public ControlAccessTypes[] accessTypes;
    }
    
    Dictionary<PlayerManager.Player, List<ControlFragment>> PlayersFlippers = new();
    Dictionary<Flipper, GameObject> FlippersPointers = new();

    void DistributeFlippers()
    {
        PlayersFlippers.Clear();
        var playerBuffer = PlayerManager.Players.ToList();
        if (playerBuffer.Count == 0)
            return;
        PlayersFlippers = playerBuffer.ToDictionary(_ => _, _ => new List<ControlFragment>());
        foreach (var flipper in Flipper.AllFlippers)
        {
            PlayersFlippers[playerBuffer.First()].Add(new ControlFragment(){flipper = flipper, accessTypes = new[]{ControlAccessTypes.Rotation}});
            refill();

            if (flipper is MovingFlipper)
            {
                PlayersFlippers[playerBuffer.First()].Add(new ControlFragment(){flipper = flipper, accessTypes = new[]{ControlAccessTypes.Movement}});
                refill();
            }

            void refill()
            {
                playerBuffer.RemoveAt(0);
                if (playerBuffer.Count == 0)
                    playerBuffer = PlayerManager.Players.ToList();
            }
        }
        
        // 'incoming flipper' pointers//
        foreach (var kvp in FlippersPointers)
            Destroy(kvp.Value);
        FlippersPointers.Clear();

        foreach (var flipper in Flipper.AllFlippers)
        {
            var pointer = Instantiate(pointerPrefab, transform.GetChild(0));
            if (flipper is not MovingFlipper)
                Destroy(pointer.transform.Find("Movement").gameObject);
            pointer.transform.localScale = new Vector3(0, 0, 1);
            FlippersPointers.Add(flipper, pointer);

            foreach (var kvp in PlayersFlippers)
            {
                foreach (var frag in kvp.Value)
                {
                    if (frag.flipper == flipper)
                    {
                        if (frag.accessTypes.Any(_ => _ == ControlAccessTypes.Rotation))
                        {
                            pointer.transform.Find("Rotation").GetComponent<Image>().color = kvp.Key.ChosenPlayerEntrySet.color;
                            frag.flipper.GetComponent<SpriteRenderer>().color = kvp.Key.ChosenPlayerEntrySet.color;
                        }
                        if (frag.accessTypes.Any(_ => _ == ControlAccessTypes.Movement))
                        {
                            pointer.transform.Find("Movement").GetComponent<Image>().color = kvp.Key.ChosenPlayerEntrySet.color;
                            (frag.flipper as MovingFlipper).lr.startColor = kvp.Key.ChosenPlayerEntrySet.color;
                            (frag.flipper as MovingFlipper).lr.endColor = kvp.Key.ChosenPlayerEntrySet.color;
                        }
                    }
                }
            }
        }
    }
}