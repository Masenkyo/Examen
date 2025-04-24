using System.Collections;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public static Follow reference;
    
    public GameObject ballObject;
    [HideInInspector]
    public GameObject trackingObject;
    [HideInInspector]
    public bool stay = false;
    public bool trackingBall = false;

    float speed;
    float originalSpeed;
    float catchupSpeed = 40f;

    [HideInInspector]
    public float distanceY;

    void Awake()
    {
        reference = this;
        originalSpeed = speed;
        TrackBall();
    }

    void Update()
    {
        trackingBall = trackingObject.TryGetComponent<Ball>(out var b);

        var trackDirection = new Vector3(0, Camera.main.transform.position.y) - new Vector3(0, trackingObject.transform.position.y);
        trackDirection.z = 0;
        distanceY = (new Vector3(0, ballObject.transform.position.y) - new Vector3(0, Camera.main.transform.position.y)).magnitude;
        float distanceCamLockY = (new Vector3(0, trackingObject.transform.position.y) - new Vector3(0, Camera.main.transform.position.y)).magnitude;
        speed = (speed > 0.3f && distanceY < 2f) ? ballObject.GetComponent<Ball>().rigidBody.linearVelocity.magnitude * 0.9f : 40;

        if (distanceY > 0.5f && !stay)
        {
            if(b == null || !b.canSpawn) Camera.main.transform.position -= trackDirection.normalized * (speed * Time.deltaTime);
            if (distanceCamLockY < 0.25f && trackingObject.TryGetComponent<LockPoint>(out _)) stay = true;
        }
        if (!trackingObject.TryGetComponent<LockPoint>(out _)) stay = false;

        if (trackingBall && b.canSpawn)
        {
            b.canSpawn = false;
            StartCoroutine(MoveUpWithDelay(b, 1f));
        }
    }

    public void TrackBall()
    {
        trackingObject = ballObject;
    }

    IEnumerator MoveUpWithDelay(Ball b, float delay)
    {
        yield return new WaitForSeconds(delay);
        b.Disable?.Invoke();
        b.Enable?.Invoke();
    }
}

