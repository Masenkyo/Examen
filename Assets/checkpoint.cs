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

        if (rotation < 2.89f)
        {
            rotation += 1 * Time.deltaTime;
            flagTransform.eulerAngles = new Vector3(0, rotation, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            checkPoint = true;
            ball.startPosition = transform.position + new Vector3(0, 1);
        }
    }
}
