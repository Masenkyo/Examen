using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MovingFlipper : Flipper
{
    [SerializeField] Transform point1, point2, point3, point4;
    [SerializeField] int movementSpeed;
    float distance;
    [HideInInspector] public LineRenderer lr;
    
    protected override void Awake()
    {
        base.Awake();
        
        var a = GetComponent<LineRenderer>();
		a.SetPosition(0, point1.position);
		a.SetPosition(1, point2.position);
		a.SetPosition(2, point3 != null ? point3.position : point1.position);
		a.SetPosition(3, point4 != null ? point4.position : point2.position);
        lr = a;
        rigidbody.position = point1.position;

        if (a.GetPosition(1) == a.GetPosition(2))
        {
            Vector3[] posses = new Vector3[a.positionCount];
            a.GetPositions(posses);
            var aslist = posses.ToList();
            aslist.RemoveAt(1);
            a.SetPositions(aslist.ToArray());
        }
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
        Vector3? bottom = point3 != null ? point3.position : null;
        Vector3? mid = point4 != null ? point4.position : null;//
       
        bool leftright = Vector3.Distance((left - (Vector3)rigidbody.position).normalized, (left - right).normalized) < 0.05f;

        bool middown = false;
        if (bottom != null && mid != null)
            middown = Vector3.Distance(((Vector3)bottom - (Vector3)rigidbody.position).normalized, ((Vector3)bottom - (Vector3)mid).normalized) < 0.05f;
        
        if (mid != null && bottom != null && (Vector3.Distance(rigidbody.position, (Vector3)mid) < 0.1f || Vector3.Distance(rigidbody.position, (Vector3)bottom) < 0.1f))
            middown = true;
        if (Vector3.Distance(rigidbody.position, left) < 0.1f || Vector3.Distance(rigidbody.position, right) < 0.1f)
            leftright = true;
        
        List<Vector3> options = new();
        if (leftright)
            options.AddRange(new[] { left, right });
        if (middown)
            options.AddRange(new[] { (Vector3)mid, (Vector3)bottom });

        if (options.Count == 0)
            return;
        
        // if (AltInputKeyboard is { } b)//
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