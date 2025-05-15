using UnityEngine;

public class checkpoint : MonoBehaviour
{
    Transform flagTransform;
    float rotation = -90f;
    bool checkPoint = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flagTransform = gameObject.transform.GetChild(0).transform;
        rotation = flagTransform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkPoint) return;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            checkPoint = true;
            flagTransform.transform.eulerAngles = Vector3.zero;
            ball.startPosition = transform.position + new Vector3(transform.position.x < 0 ? 3.2f : -3.2f, 1);
        }
    }
}
