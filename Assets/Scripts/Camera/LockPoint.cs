using UnityEngine;

public class LockPoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Ball>(out var ball)) return;

        Debug.Log("enter");
        Follow.reference.trackingObject = gameObject;  
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Ball>(out var ball)) return;

        Debug.Log("exit");
        Follow.reference.TrackBall();
    }
}
