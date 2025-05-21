using System.Linq;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public static Follow reference;
    
    float speed;
    float catchupSpeed = 40f;

    Ball ball;
    
    void Awake()
    {
        reference = this;
        ball = FindAnyObjectByType<Ball>();
    }

    void Update()
    {
        float? some = FindObjectsByType<CameraZone>(FindObjectsSortMode.None).FirstOrDefault(_ => _.GetComponent<Collider2D>().bounds.Contains(ball.transform.position) &&
            _.transform.position.y < ball.transform.position.y)?.transform.position.y;
        
        var target = some ?? ball.transform.position.y;
        var moveDir = (target - Camera.main.transform.position.y) switch { > 0 => 1, < 0 => -1, _ => 0 };
        var distance = Mathf.Abs(target - Camera.main.transform.position.y);
        
        speed = (speed > 0.3f && distance < 2f) ? ball.rigidBody.linearVelocity.magnitude * 0.9f : 40;

        if (distance > 0.5f)
            Camera.main.transform.position += new Vector3(0, moveDir * (speed * Time.deltaTime));
    }
}

