using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GameManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void DoNothing() { }
    static GameManager()
    {
        GameStateClass.OnGameStateChanged += _ =>
        {
            if (_ == GameStates.InGame)
                GameStarted();
            else 
                GameEnded();
        };
    }

    static Dictionary<PlayerManager.Player, List<Flipper>> PlayersFlippers = new();

    static void GameStarted()
    {
        DistributeFlippers();

        var rc = InputSystem.actions.FindActionMap("Main").FindAction("Rotate");
        rc.Enable();
        
        var cancel= new Action(() =>
        {
            foreach (var keyValuePair in PlayersFlippers)
                keyValuePair.Value.ForEach(_ =>
                {
                    _.doubleSpeedPressed = keyValuePair.Key.Gamepad.squareButton.isPressed;
                    _.DesiredHorizontalMovement = keyValuePair.Key.Gamepad.leftStick.value.x;
                });
        }).AddToUpdate();
        
        GameStateClass.OnGameStateChanged = once + GameStateClass.OnGameStateChanged;
        void once(GameStates _)
        {
            GameStateClass.OnGameStateChanged -= once;
            cancel();
        }
    }

    static void GameEnded()
    {
        PlayersFlippers = new();
    }

    static void DistributeFlippers()
    {
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