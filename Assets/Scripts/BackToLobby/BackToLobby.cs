using System.Collections;
using UnityEngine;

public class BackToLobby : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(2.5f);
            GameStateClass.GameState = GameStates.InLobby;
        }
    }
}