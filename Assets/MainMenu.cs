using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button StartButton;

    void Awake() => StartButton.onClick.AddListener(() => { GameStateClass.GameState = GameStates.InLobby; });
}