using UnityEngine;
using UnityEngine.InputSystem;

public class MovingFlipper : Flipper
{
    [SerializeField] Transform point1, point2;
    [SerializeField] int movementSpeed;
    float distance;
    
    void Start()
    {
        var a = GetComponent<LineRenderer>();
		a.SetPosition(0, point1.position);
		a.SetPosition(1, point2.position);
    }
    
    override protected void Update()
    {
        if (Gamepad.current is { } c)
            InputJoystickMovement = c.leftStick.value;
        base.Update();
        MoveFlipper();
    }

    public Vector3 InputJoystickMovement; 
    
    void MoveFlipper()
    {
        // rigidbody.position = DesiredHorizontalMovement < 0 && !brokenFlipper
        //     ? movementSpeed * doubleSpeed * -DesiredHorizontalMovement
        //     : DesiredHorizontalMovement > 0 && !brokenFlipper
        //         ? -movementSpeed * doubleSpeed * DesiredHorizontalMovement
        //         : 0;

        //transform.position = Vector3.Lerp(point1.position, point2.position, distance);

        var DirTo1 = (point1.position - (Vector3)rigidbody.position).normalized; 
        var DirTo2 = (point2.position - (Vector3)rigidbody.position).normalized;

        // going to 1

        if (InputJoystickMovement == Vector3.zero)//
            return;
        
        if (Vector3.Distance(InputJoystickMovement, DirTo1) < Vector3.Distance(InputJoystickMovement, DirTo2))
        {
            rigidbody.position +=
                (Vector2)((point1.position - (Vector3)rigidbody.position).normalized * (Time.deltaTime * movementSpeed * doubleSpeed * (1 - Vector3.Distance(InputJoystickMovement, DirTo1) + 1)));
        }
        else // going to 2
        {
            rigidbody.position += 
                (Vector2)((point2.position - (Vector3)rigidbody.position).normalized * (Time.deltaTime * movementSpeed * doubleSpeed * (1 - Vector3.Distance(InputJoystickMovement, DirTo2) + 1)));
        }
      
    }
}