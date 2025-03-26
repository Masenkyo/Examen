using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityEvent LastMoments;
    [HideInInspector]
    public Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    public Vector3 startPosition = new Vector3(-6, 7.41f);
    Vector3 impactVelocity;

    [SerializeField]
    float damagableVelocity;
    [HideInInspector]
    public bool canSpawn = false;

    [SerializeField]
    float maxDurability = 35f;
    float Durability
    {
        get => _durability ??= maxDurability;
        set
        {
            _durability = value;
            if (value <= 0) LastMoments.Invoke();
        }
    } float? _durability;

    void FirstLastMoment()
    {
        canSpawn = true;
        transform.localScale = new Vector3(0, 0);
    }

    public void ResetBall()
    {
        canSpawn = false;
        transform.localScale = new Vector3(1, 1);
        rigidBody.linearVelocity = Vector3.zero;
        spriteRenderer.color = Color.red;
        transform.position = startPosition;
        Durability = maxDurability;
    }

    void Awake()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        LastMoments.AddListener(FirstLastMoment);
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
            spriteRenderer.color = new Vector4(Durability / maxDurability, 0, 0, 1);
        }
    }
}
