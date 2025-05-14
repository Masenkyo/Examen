using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public static Ball reference;
    public UnityEvent Enable;
    public UnityEvent Disable;

    float teleportWidth;
    
    [HideInInspector]
    public Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    public Vector3 startPosition = new(-6, 7.41f);
    Vector3 impactVelocity;
    Phases phases;

    [SerializeField]
    float damagableVelocity;
    [HideInInspector]
    public bool canSpawn;

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
        foreach(var p in Powerup.allPowerups)
        {
            p.waitTime = 0;
        }

        spriteRenderer.enabled = false;
        rigidBody.simulated = false;
        rigidBody.linearVelocity = new(0, 0);
        rigidBody.angularVelocity = 0;
        GetComponent<ParticleSystem>().Play();
        canSpawn = true;
    }

    bool ready = true;
    float time;
    [SerializeField] float moveTime;
    
    void EnableBall()
    {
        foreach(var p in Powerup.allPowerups)
        {
            p.SetPowerup();
            p.waitTime = 10;
        }

        canSpawn = false;
        GetComponent<ParticleSystem>().Stop();
        spriteRenderer.color = Color.red;
        spriteRenderer.enabled = true;
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = 0;
        phases.ResetPhases();
        Durability = maxDurability;
        StartCoroutine(WaitForDrop());
    }

    float beginTime = 5f;
    bool firstTime = true;
    
    IEnumerator WaitForDrop(float waitTime = 3f)
    {
        ready = true;
        moveTime = waitTime / 1.2f;

        if (firstTime)
        {
            yield return new WaitForSeconds(beginTime);
            firstTime = false;
        }
        
        yield return new WaitForSeconds(waitTime);
        
        rigidBody.simulated = true;
        ready = false;
    }

    void Awake()
    {
        reference = this;
        teleportWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x  + 1;
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        phases = GetComponent<Phases>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Enable.AddListener(EnableBall);
        Disable.AddListener(DisableBall);
    }

    void Start()
    {
        DisableBall();
        EnableBall();
    }

    void Update()
    {
        impactVelocity = rigidBody.GetPointVelocity(transform.position);
        if (ready)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition + new Vector3(startPosition.x < 0 ? -2 : 2, 0, 0), startPosition, time / moveTime);
        }
        else time = 0;
        
        TeleportSideScreen(teleportWidth);

        if (transform.position.y < -5 + LevelSystem.instance.amountOfLevels * LevelSystem.instance.gapBetweenLevels)
            Durability = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (impactVelocity.magnitude > damagableVelocity)
        {
            Durability -= impactVelocity.magnitude;
        }
    }

    Vector3 teleportLocation;
    
    void TeleportSideScreen(float x = 2f)
    {
        var pos = transform.position;
        pos.x = transform.position.x > x ? -x : transform.position.x < -x ? x : transform.position.x;
        transform.position = pos;
    }
}
