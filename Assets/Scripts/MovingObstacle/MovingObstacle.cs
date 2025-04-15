using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField]
    List<Transform> path = new List<Transform>();
    [SerializeField]
    float speed = 1;
    int pathIndex = 0;

    void Update()
    {
        var targetDirection = (path[pathIndex].position - transform.position).normalized;
        float distance = (path[pathIndex].position - transform.position).magnitude;
        transform.position += targetDirection * (speed * Time.deltaTime);

        if(distance < 0.1f)
        {
            pathIndex++;
            if (pathIndex > path.Count-1) pathIndex = 0;
        }
    }
}
