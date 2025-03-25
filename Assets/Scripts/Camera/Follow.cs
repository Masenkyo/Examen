using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public static Follow reference;

    [SerializeField]
    GameObject ball;
    [HideInInspector]
    public GameObject trackingObject;
    bool stay = false;

    float speed;
    float originalSpeed;
    float catchupSpeed = 18f;

    void Awake()
    {
        reference = this;
        originalSpeed = speed;
        TrackBall();
    }

    void Update()
    {
        speed = speed > 0.1 ? ball.GetComponent<Ball>().rigidBody.linearVelocity.magnitude * 0.9f : catchupSpeed;

        Vector3 ballDirection = new Vector3(0, Camera.main.transform.position.y) - new Vector3(0, trackingObject.transform.position.y);
        ballDirection.z = 0;

        float distanceY = (new Vector3(0, ball.transform.position.y) - new Vector3(0, Camera.main.transform.position.y)).magnitude;

        if (distanceY > 0.5f && !stay)
        {
            Camera.main.transform.position -= ballDirection.normalized * (speed * Time.deltaTime);
            if (distanceY < 0.01 && trackingObject.TryGetComponent<LockPoint>(out _)) stay = true;
        }
        if (!trackingObject.TryGetComponent<LockPoint>(out _)) stay = false;
    }

    public void TrackBall()
    {
        trackingObject = ball;
    }
}
