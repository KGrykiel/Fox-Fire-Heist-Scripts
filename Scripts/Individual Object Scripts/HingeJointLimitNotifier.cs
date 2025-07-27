using UnityEngine;
using UnityEngine.Events;

public class HingeJointLimitNotifier : MonoBehaviour
{
    public HingeJoint hingeJointComponent;
    public UnityEvent onMaxLimitReached;
    public UnityEvent onMinLimitReached;

    private bool maxLimitReached = false;
    private bool minLimitReached = false;

    void Start()
    {
        if (hingeJointComponent == null)
        {
            hingeJointComponent = GetComponent<HingeJoint>();
        }
    }

    void Update()
    {
        // Check if the hinge joint has reached its maximum limit
        if (hingeJointComponent.angle >= hingeJointComponent.limits.max && !maxLimitReached)
        {
            maxLimitReached = true;
            onMaxLimitReached.Invoke();
        }
        else if (hingeJointComponent.angle < hingeJointComponent.limits.max)
        {
            maxLimitReached = false;
        }

        // Check if the hinge joint has reached its minimum limit
        if (hingeJointComponent.angle <= hingeJointComponent.limits.min && !minLimitReached)
        {
            minLimitReached = true;
            onMinLimitReached.Invoke();
        }
        else if (hingeJointComponent.angle > hingeJointComponent.limits.min)
        {
            minLimitReached = false;
        }
    }

}

