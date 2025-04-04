using System.Collections;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public static Follow reference;

    [SerializeField]
    GameObject ballObject;
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
        distanceY = (new Vector3(0, ballObject.transform.position.y) - new Vector3(0, Camera.main.transform.position.y)).magnitude;
        float distanceCamLockY = (new Vector3(0, trackingObject.transform.position.y) - new Vector3(0, Camera.main.transform.position.y)).magnitude;
        speed = (speed > 0.1f && distanceY < 2f) ? ballObject.GetComponent<Ball>().rigidBody.linearVelocity.magnitude * 0.9f : 
            catchupSpeed = distanceY > (LevelSystem.instance.gapBetweenLevels * LevelSystem.instance.amountOfLevels) ? 200 : 40;

        if (distanceY > 0.5f && !stay)
        {
            if(b == null || !b.canSpawn) Camera.main.transform.position -= trackDirection.normalized * (speed * Time.deltaTime);
            if (distanceCamLockY < 0.1 && trackingObject.TryGetComponent<LockPoint>(out _)) stay = true;
        }
        if (!trackingObject.TryGetComponent<LockPoint>(out _)) stay = false;

        if (trackingBall && b.canSpawn) StartCoroutine(MoveUpWithDelay(b, 1f));
    }

    public void TrackBall()
    {
        trackingObject = ballObject;
    }

    IEnumerator MoveUpWithDelay(Ball b, float delay)
    {
        Vector3 originDirection = new Vector3(0, Camera.main.transform.position.y) - new Vector3(0, b.startPosition.y);
        if (originDirection.magnitude < 2) b.Enable.Invoke();
        yield return new WaitForSeconds(delay);
        Camera.main.transform.position -= originDirection.normalized * (speed * Time.deltaTime);
    }
}

