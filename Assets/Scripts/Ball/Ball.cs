using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    public UnityEvent LastMoments;
    [HideInInspector]
    public Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Vector3 startPosition = new Vector3(-6, 7.41f);
    Vector3 impactVelocity;

    [SerializeField]
    float damagableVelocity;

    float Durability
    {
        get => _durability;
        set
        {
            _durability = value;
            if (value <= 0) LastMoments.Invoke();
        }
    }
    float _durability = maxDurability;
    const float maxDurability = 35f;

    void Destr()
    {
        rigidBody.linearVelocity = Vector3.zero;
        transform.position = startPosition;
        _durability = maxDurability;
    }

    void Awake()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        LastMoments.AddListener(Destr);
    }

    void Update()
    {
        impactVelocity = rigidBody.GetPointVelocity(transform.position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (impactVelocity.magnitude > damagableVelocity)
        {
            Durability -= impactVelocity.magnitude;
            spriteRenderer.color = new Vector4(_durability / maxDurability, 0, 0, 1);
        }
    }
}
