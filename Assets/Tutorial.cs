using System.Collections;
using UnityEngine;
public class Tutorial : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(25);
            GameStateClass.GameState = GameStates.InGame;
        }
    }
    
    public void SkipButton()
    {
        StopAllCoroutines();
        GameStateClass.GameState = GameStates.InGame;
    }
}