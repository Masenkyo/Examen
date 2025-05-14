using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MovingFlipper : Flipper
{
    [SerializeField] Transform point1, point2, point3;
    [SerializeField] int movementSpeed;
    float distance;
    public LineRenderer lr;
    
    protected override void Awake()
    {
        base.Awake();
        
        var a = GetComponent<LineRenderer>();
		a.SetPosition(0, point1.position);
		a.SetPosition(1, point2.position);
		a.SetPosition(2, (point2.position + point1.position) / 2);
		a.SetPosition(3, point3.position);
        lr = a;
        rigidbody.position = point1.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveFlipper();
    }

    public Vector3 InputJoystickMovement;
    public bool? AltInputKeyboard;
    
    void MoveFlipper()
    {
	    if (brokenFlipper)
		    return;
        if (InputJoystickMovement == Vector3.zero)
            return;
        
        var DirTo1 = (point1.position - (Vector3)rigidbody.position).normalized; 
        var DirTo2 = (point2.position - (Vector3)rigidbody.position).normalized;

        var left = point1.position;
        var right = point2.position;
        var bottom = point3.position;
        var mid = (left + right) / 2;
       
        bool leftright = Vector3.Distance((left - (Vector3)rigidbody.position).normalized, (left - right).normalized) < 0.05f;
        bool middown = Vector3.Distance((bottom - (Vector3)rigidbody.position).normalized, (bottom - mid).normalized) < 0.05f;
        if (Vector3.Distance(rigidbody.position, mid) < 0.1f || Vector3.Distance(rigidbody.position, bottom) < 0.1f)
            middown = true;
        if (Vector3.Distance(rigidbody.position, left) < 0.1f || Vector3.Distance(rigidbody.position, right) < 0.1f)
            leftright = true;
        
        List<Vector3> options = new();
        if (leftright)
            options.AddRange(new[] { left, right });
        if (middown)
            options.AddRange(new[] { mid, bottom });

        if (options.Count == 0)
            return;
        
        // if (AltInputKeyboard is { } b)
        // {
        //    if (Time.deltaTime * movementSpeed * 2 is { } step &&
        //        step > Vector3.Distance(rigidbody.position, (b ? chosenPoint1 : chosenPoint2)))
        //        rigidbody.position += ((b ? chosenPoint1 : chosenPoint2) - rigidbody.position).normalized * step;
        //    else
        //        rigidbody.position = b ? chosenPoint1 : chosenPoint2;
        //    return;
        // }
//
        var closest = options.OrderBy(_ => Vector3.Distance(InputJoystickMovement, (_ - (Vector3)rigidbody.position).normalized )).First();


        if (Time.deltaTime * movementSpeed * InputJoystickMovement.magnitude is { } step
            && Vector3.Distance(rigidbody.position, closest) > step)
        {
            rigidbody.position += (Vector2)(closest - (Vector3)rigidbody.position).normalized * step;
        }
        else
           rigidbody.position = closest;
      
    }
}