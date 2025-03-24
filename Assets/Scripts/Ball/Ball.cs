using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityEvent LastMoments;
        Rigidbody2D rigidBody;
    Vector3 startPosition;
    Vector3 previousVelocity;

    [SerializeField]
    float damagableVelocity = 30f;
    
    float Durability
    {
        get => _durability;
        set => _durability = value;
    } float _durability = 100f;

    void Destr()
    {
        rigidBody.linearVelocity = Vector3.zero;
        transform.position = startPosition;
    }

    void Awake()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        LastMoments.AddListener(Destr);
    }

    void Update()
    {
        previousVelocity = rigidBody.linearVelocity;
        // Debug.Log(previousVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("damagablevelocity " + damagableVelocity + " velocity " + previousVelocity.magnitude);
        if (previousVelocity.magnitude > damagableVelocity)
        {
            Debug.Log("damage");
            _durability -= previousVelocity.magnitude;
            GetComponent<SpriteRenderer>().color = new Vector4(_durability / 100, 0, 0, 1);
            if (_durability <= 0)
            {
                Debug.Log("event");
                LastMoments.Invoke();
            }
        }
    }
}
