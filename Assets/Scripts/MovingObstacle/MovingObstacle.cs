using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField]
    List<Transform> path = new List<Transform>();
    Transform targetPoint;
    float speed = 1;

    void Start()
    {
        targetPoint = path[0];    
    }

    void Update()
    {

        Vector3 target = (transform.position - targetPoint.position).normalized;
        //float distance = (transform.position)
        transform.position += target * (speed * Time.deltaTime);
    }
}
