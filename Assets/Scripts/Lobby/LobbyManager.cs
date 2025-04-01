using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public static class PlayerManager
{
    [Serializable]
    public struct PlayerEntrySet
    {
        public Sprite icon;
        public Color color;
        public string id;
        
        public static bool operator ==(PlayerEntrySet a, PlayerEntrySet b) => a.color == b.color;
        public static bool operator !=(PlayerEntrySet a, PlayerEntrySet b) => !(a == b);
    }
    
    public class Player
    {
        public bool IsKing
        {
            get => LP.IsKing;
            set => LP.IsKing = value;
        }
        public PlayerEntrySet ChosenPlayerEntrySet;
        public LobbyPlayer LP;
        public Gamepad Gamepad;
        public float AddedDate;
    }
    
    public static readonly List<Player> Players = new();
    static Player Oldest => Players.Count == 0 ? null : Players.OrderBy(_ => _.AddedDate).First();
    public static Player CurrentKing
    {
        get
        {
            foreach (Player t in Players)
                if (t.IsKing)
                    return t;

            Player o = Oldest;
            if (o is { })
                o.IsKing = true;
            return o;
        }
        set
        {
            CurrentKing.IsKing = false;
            value.IsKing = true;
        }
    } static Player _currentKing;
}

public class LobbyManager : MonoBehaviour
{
    // references
    [SerializeField] 
    GameObject lobbyPlayerPrefab, inGamePlayerPrefab;
    [SerializeField] 
    PlayerManager.PlayerEntrySet[] lobbyPlayerParams;
    
    [SerializeField] 
    public LobbyPlayerList inLobbyPlayerList;

    public GameObject InGameUI => refer.inGameUI;
    
    [SerializeField] public GameObject inGameUI;
    [SerializeField] public GameObject inLobbyUI;
    
    
    [SerializeField] 
    public LobbyPlayerList inGamePlayerList;
    
    public static Action<PlayerManager.Player> PlayerAddedAfter = _ => { };
    public static Action<PlayerManager.Player> PlayerRemovedBefore = _ => { };

    LobbyPlayer CreateLP(PlayerManager.PlayerEntrySet chosen)
    {
        GameObject createdLP  = (GameStateClass.GameState == GameStates.InLobby
                ? GameObject.Instantiate(lobbyPlayerPrefab, inLobbyPlayerList.transform)
                : GameObject.Instantiate(inGamePlayerPrefab, inGamePlayerList.transform));
        
        if (GameStateClass.GameState == GameStates.InGame)
            inGamePlayerList.invitation.SetAsLastSibling();
        
        LobbyPlayer LPComp = createdLP.GetComponent<LobbyPlayer>();
        LPComp.Color = chosen.color;
        LPComp.avatar.sprite = chosen.icon;
        LPComp.id.text = chosen.id;

        return LPComp;
    }
    
    void AddPlayer(Gamepad gamepad)
    {
        if (PlayerManager.Players.Count == 4)
            return;
        
        PlayerManager.PlayerEntrySet chosen = lobbyPlayerParams.First(_ => !PlayerManager.Players.Any(__ => __.ChosenPlayerEntrySet == _));
        
        PlayerManager.Player player = new()
        {
            Gamepad = gamepad,
            LP = CreateLP(chosen),
            IsKing = PlayerManager.Players.Count == 0,
            AddedDate = Time.time,
            ChosenPlayerEntrySet = chosen
        };
        PlayerManager.Players.Add(player);
        
        PlayerAddedAfter(player);
    }

    void RemovePlayer(PlayerManager.Player player)
    {
        PlayerRemovedBefore(player);
        
        Object.Destroy(player.LP.gameObject);
        player.LP = null;
        
        PlayerManager.Players.Remove(player);
        _ = PlayerManager.CurrentKing;
    }
    
    static bool doneSetup;
    void StaticSetup()
    {
        doneSetup = true;
     
        PlayerAddedAfter += _ => { invitationRefresh(); };
        PlayerRemovedBefore += _ => { invitationRefresh(); };
        void invitationRefresh()
        {
            switch (GameStateClass.GameState)
            {
                case GameStates.InLobby:
                    refer?.inLobbyPlayerList.invitation.gameObject.SetActive(PlayerManager.Players.Count < 4);
                    break;
                case GameStates.InGame:
                    refer?.inGamePlayerList.invitation.gameObject.SetActive(PlayerManager.Players.Count < 4);
                    break;
            }
        }
        
        GameStateClass.OnGameStateChanged += (_ =>
        {
            invitationRefresh();

            if (_ == GameStates.InMenu)
            {
                Return.Disable();
                PlayerManager.Players.Clear();
            }
            else
                Return.Enable();
            
            if (_ == GameStates.InLobby)
                StartGame.Enable();
            else
                StartGame.Disable();
            
            // UIs
            InGameUI.SetActive(GameStateClass.GameState == GameStates.InGame);
            inLobbyUI.SetActive(GameStateClass.GameState == GameStates.InLobby);
            
            // Back to menu? Kill everyone.
            if (GameStateClass.GameState == GameStates.InMenu)
                PlayerManager.Players.Clear();
            
            // Transfer LPs
            PlayerManager.Players.ForEach(_ =>
            {
                if (_.LP != null)
                    GameObject.Destroy(_.LP.gameObject);
            });
            PlayerManager.Players.ForEach(_ => _.LP = CreateLP(_.ChosenPlayerEntrySet));
            PlayerManager.CurrentKing = PlayerManager.CurrentKing;
        });
    }

    static InputAction StartGame => _startGame ??= InputSystem.actions.FindActionMap("Main").FindAction("Start Game");
    static InputAction _startGame;
    
    static InputAction Return => _return ??= InputSystem.actions.FindActionMap("Main").FindAction("Return to Lobby/MainMenu");
    static InputAction _return;
    
    static InputAction JoinGame => _joinGame ??= InputSystem.actions.FindActionMap("Main").FindAction("Join Game");
    static InputAction _joinGame;
    
    static InputAction LeaveGame => _leaveGame ??= InputSystem.actions.FindActionMap("Main").FindAction("Leave Game");
    static InputAction _leaveGame;
    
    static LobbyManager refer => Object.FindAnyObjectByType<LobbyManager>();
    void Awake()
    {
        if (Object.FindObjectsByType<LobbyManager>(FindObjectsSortMode.None).Length == 2)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
        if (!doneSetup)
            StaticSetup();
        
        StartGame.performed += context =>
        {
            if (PlayerManager.Players.FirstOrDefault(_ => _.Gamepad == context.control.device) is not { IsKing: true })
                return;
            GameStateClass.GameState = GameStates.InGame;   
        };

        Return.performed += context =>
        {
            if (PlayerManager.Players.FirstOrDefault(_ => _.Gamepad == context.control.device) is not { IsKing: true })
                return;
            GameStateClass.GameState = GameStates.InMenu;
        };
        
        JoinGame.performed += context =>
        {
            if (PlayerManager.Players.Any(_ => _.Gamepad == context.control.device))
                return;
            AddPlayer(context.control.device as Gamepad);
        };

        LeaveGame.performed += context =>
        {
            if (PlayerManager.Players.FirstOrDefault(_ => _.Gamepad == context.control.device) is { } f) 
                RemovePlayer(f);
        };
    }

    // Track gamepad connect/disconnect
    void Update()
    {
        // Check if some gamepad disconnected
        foreach (PlayerManager.Player player in PlayerManager.Players.ToList())
            if (!Gamepad.all.Contains(player.Gamepad))
                RemovePlayer(player);
    }
}


