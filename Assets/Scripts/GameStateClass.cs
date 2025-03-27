using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameStateClass
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void DoNothing()
    {
        _ = GameState;
    }
    
    public static GameStates GameState
    { 
        get => _gameState ??= SceneManager.GetActiveScene().name switch
        {
            "MainMenu" => GameStates.InMenu,
            "Lobby" => GameStates.InLobby,
            "Flippers" => GameStates.InGame,
        };
        set
        {
            if (_gameState == value)
                return;

            SceneManager.sceneLoaded += wait;
            SceneManager.LoadScene(value switch
            {
                GameStates.InMenu => "MainMenu",
                GameStates.InLobby => "Lobby",
                GameStates.InGame => "Flippers"
            });
            void wait(Scene arg0, LoadSceneMode loadSceneMode)
            {
                SceneManager.sceneLoaded -= wait;
                
                _gameState = value;
                OnGameStateChanged(value);
            }
        }
    } static GameStates? _gameState;
    public static Action<GameStates> OnGameStateChanged = _ => { };
}

public enum GameStates
{
    InMenu, InLobby, InGame
}