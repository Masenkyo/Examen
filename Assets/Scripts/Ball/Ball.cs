using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityEvent Enable;
    public UnityEvent Disable;
    [HideInInspector]
    public Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    public Vector3 startPosition = new Vector3(-6, 7.41f);
    Vector3 impactVelocity;
    Phases phases;

    [SerializeField]
    float damagableVelocity;
    [HideInInspector]
    public bool canSpawn = false;

    [SerializeField]
    float maxDurability = 35f;
    public float getMaxDurability
    {
        get => maxDurability;
    }

    public float Durability
    {
        get => _durability ??= maxDurability;
        set
        {
            _durability = value;
            if (value <= 0) Disable.Invoke();
        }
    } float? _durability;

    void DisableBall()
    {
        spriteRenderer.enabled = false;
        rigidBody.simulated = false;
        rigidBody.linearVelocity = new(0, 0);
        GetComponent<ParticleSystem>().Play();
        canSpawn = true;
    }

    bool ready;
    float time;
    [SerializeField] float moveTime;
    
    void EnableBall()
    {
        canSpawn = false;
        GetComponent<ParticleSystem>().Stop();
        spriteRenderer.color = Color.red;
        spriteRenderer.enabled = true;
        rigidBody.linearVelocity = Vector3.zero;
        ready = true;
        phases.ResetPhases();
        Durability = maxDurability;
        StartCoroutine(WaitForDrop());
    }
    
    IEnumerator WaitForDrop(float waitTime = 3f)
    {
        moveTime = waitTime / 1.2f;
        yield return new WaitForSeconds(waitTime);
        
        rigidBody.simulated = true;
        ready = false;
    }

    void Awake()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        phases = GetComponent<Phases>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Enable.AddListener(EnableBall);
        Disable.AddListener(DisableBall);
    }

    void Update()
    {   


        impactVelocity = rigidBody.GetPointVelocity(transform.position);
        if (ready)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition + new Vector3(2, 0, 0), startPosition, time / moveTime);
        }
        else time = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (impactVelocity.magnitude > damagableVelocity)
        {
            Durability -= impactVelocity.magnitude;
        }
    }
}