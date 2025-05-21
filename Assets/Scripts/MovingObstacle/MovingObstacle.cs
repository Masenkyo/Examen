using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField]
    List<Transform> path = new List<Transform>();
    [SerializeField]
    float speed = 1;
    int pathIndex = 0;
    float rotation = 0;
    [SerializeField] bool isKnife;
    [SerializeField]
    List<Sprite> knives = new List<Sprite>();

    void Awake()
    {
        if (path.Count < 1) path.Add(transform);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = knives[Random.Range(0, knives.Count)];
        if (isKnife)
        {
        Vector2 spriteSize = spriteRenderer.bounds.size;
        GetComponent<BoxCollider2D>().size = spriteSize;
        }
    }

    void Update()
    {
        var targetDirection = (path[pathIndex].position - transform.position).normalized;
        transform.up = -targetDirection;

        float distance = (path[pathIndex].position - transform.position).magnitude;
        if(distance > 0.1f)
            transform.position += targetDirection * (speed * Time.deltaTime);
        else
        {
            pathIndex++;
            if (pathIndex > path.Count - 1) pathIndex = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            ball.Durability = 0;
        }
    }
}
