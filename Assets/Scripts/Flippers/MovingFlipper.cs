using UnityEngine;
using UnityEngine.InputSystem;

public class MovingFlipper : Flipper
{
    [SerializeField] Transform point1, point2;
    [SerializeField] int movementSpeed;
    float distance;
    public LineRenderer lr;
    
    protected override void Awake()
    {
        base.Awake();
        
        var a = GetComponent<LineRenderer>();
		a.SetPosition(0, point1.position);
		a.SetPosition(1, point2.position);
        lr = a;
    }
    
    override protected void Update()
    {
        base.Update();
        MoveFlipper();
    }

    public Vector3 InputJoystickMovement;
    public bool? AltInputKeyboard;
    
    void MoveFlipper()
    {
        var DirTo1 = (point1.position - (Vector3)rigidbody.position).normalized; 
        var DirTo2 = (point2.position - (Vector3)rigidbody.position).normalized;

        // going to 1


        if (brokenFlipper)
            return;
        
        if (AltInputKeyboard is { } b)
        {
            if (Time.deltaTime * movementSpeed * 2 is { } step &&
                step > Vector3.Distance(rigidbody.position, (b ? point1 : point2).position))
                rigidbody.position +=
                    (Vector2)(((b ? point1 : point2).position - (Vector3)rigidbody.position).normalized * step);
            else
                rigidbody.position = (b ? point1 : point2).position;
            return;
        }
        
        if (InputJoystickMovement == Vector3.zero)
            return;
        
        if (Vector3.Distance(InputJoystickMovement, DirTo1) < Vector3.Distance(InputJoystickMovement, DirTo2))
        {
            if (Time.deltaTime * movementSpeed * (1 - Vector3.Distance(InputJoystickMovement, DirTo1) + 1) is { } step 
                && Vector3.Distance(rigidbody.position, point1.position) > step)
                rigidbody.position += (Vector2)((point1.position - (Vector3)rigidbody.position).normalized * step);
            else
                rigidbody.position = point1.position;
        }
        else // going to 2
        {
            if (Time.deltaTime * movementSpeed * (1 - Vector3.Distance(InputJoystickMovement, DirTo2) + 1) is { } step 
            && Vector3.Distance(rigidbody.position, point2.position) > step)
                rigidbody.position += (Vector2)((point2.position - (Vector3)rigidbody.position).normalized * step);
            else 
                rigidbody.position = point2.position;
        }
      
    }
}