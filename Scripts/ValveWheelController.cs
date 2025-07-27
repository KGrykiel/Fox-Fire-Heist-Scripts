using UnityEngine;
using UnityEngine.Events;

//this script is stupid and I dont like it
public class ValveWheelController : MonoBehaviour
{
    public HingeJoint valveWheelHinge;
    public Transform kneePipe;
    public UnityEvent onTurnUp; // Event for turning the pipe to the "Up" direction
    public UnityEvent onTurnDown; // Event for turning the pipe to the "Down" direction
    public UnityEvent onTurnLeft; // Event for turning the pipe to the "Left" direction
    public UnityEvent onTurnRight; // Event for turning the pipe to the "Right" direction
    public bool pipeFlip;

    private string currentState = "Left"; // Initial state

    void Start()
    {
        if (valveWheelHinge == null)
        {
            valveWheelHinge = GetComponent<HingeJoint>();
        }
    }

    void Update()
    {
        // Rotate the knee pipe alongside the valve wheel
        float wheelAngle = valveWheelHinge.angle;
        if (pipeFlip) {
            wheelAngle = -wheelAngle;
        }
        kneePipe.localRotation = Quaternion.Euler(0f, wheelAngle, 0f);

        // Check if the player has stopped holding the wheel
        if (Input.GetMouseButtonUp(0))
        {
            SnapToNearestDirection();
        }
    }

    void SnapToNearestDirection()
    {
        float wheelAngle = valveWheelHinge.angle;
        string newState = GetStateFromAngle(wheelAngle);

        // Snap the wheel and pipe to the nearest direction
        float targetAngle = GetTargetAngleFromState(newState);
        valveWheelHinge.transform.localRotation = Quaternion.Euler(0f, targetAngle, 0f);
        kneePipe.localRotation = Quaternion.Euler(0f, -targetAngle, 0f);

        // Update the state and trigger the event if the state has changed
        if (newState != currentState)
        {
            currentState = newState;
            print("new state: " + newState);

            // Invoke the specific events for turning the pipe
            if (newState == "Up")
            {
                onTurnUp.Invoke();
            }
            else if (newState == "Down")
            {
                onTurnDown.Invoke();
            }
            else if (newState == "Left")
            {
                onTurnLeft.Invoke();
            }
            else if (newState == "Right")
            {
                onTurnRight.Invoke();
            }
        }
    }

    string GetStateFromAngle(float angle)
    {
        if(!pipeFlip)
        {
            angle = -angle;
        }
        if (angle >= 45f && angle <= 135f)
        {
            return "Up";
        }
        else if (angle >= -135f && angle <= -45f)
        {
            return "Down";
        }
        else if (angle >= -45f && angle <= 45f)
        {
            return "Left";
        }
        else
        {
            return "Right";
        }
    }

    float GetTargetAngleFromState(string state)
    {
        switch (state)
        {
            case "Down":
                return 90f;
            case "Up":
                return -90f;
            case "Left":
                return 0f;
            case "Right":
                return 180f;
            default:
                return 0f;
        }
    }
}
