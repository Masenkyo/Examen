using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public static Follow reference;

    [SerializeField]
    GameObject ball;
    [HideInInspector]
    public GameObject trackingObject;
    [HideInInspector]
    public bool stay = false;
    bool trackingBall = false;

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

        Vector3 trackDirection = new Vector3(0, Camera.main.transform.position.y) - new Vector3(0, trackingObject.transform.position.y);
        trackDirection.z = 0;
        distanceY = (new Vector3(0, ball.transform.position.y) - new Vector3(0, Camera.main.transform.position.y)).magnitude;
        float distanceCamLockY = (new Vector3(0, trackingObject.transform.position.y) - new Vector3(0, Camera.main.transform.position.y)).magnitude;
        speed = (speed > 0.1f && distanceY < 2f) ? ball.GetComponent<Ball>().rigidBody.linearVelocity.magnitude * 0.9f : catchupSpeed;

        if (distanceY > 0.5f && !stay)
        {
            if(b == null || !b.canSpawn) Camera.main.transform.position -= trackDirection.normalized * (speed * Time.deltaTime);
            if (distanceCamLockY < 0.1 && trackingObject.TryGetComponent<LockPoint>(out _)) stay = true;
        }
        if (!trackingObject.TryGetComponent<LockPoint>(out _)) stay = false;

        if(trackingBall && b.canSpawn)
        {
            Vector3 originDirection = new Vector3(0, Camera.main.transform.position.y) - new Vector3(0, b.startPosition.y); 
            Camera.main.transform.position -= originDirection.normalized * (speed * Time.deltaTime);
            if (originDirection.magnitude < 2) b.ResetBall();
        }
    }

    public void TrackBall()
    {
        trackingObject = ball;
    }
}
