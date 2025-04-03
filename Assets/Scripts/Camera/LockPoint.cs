using UnityEngine;

public class LockPoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Ball>(out _)) return;
        Follow.reference.trackingObject = gameObject;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Ball>(out _)) return;
        Follow.reference.TrackBall();
    }
}
