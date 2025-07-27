using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public string animationName = "PlayAnimation"; // Name of the animation state
    public UnityEvent onBothPowerActivated; // Event to propagate when both conditions are true

    private bool leftPower = false;
    private bool rightPower = false;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("Animator component not found on " + gameObject.name);
            }
        }
    }

    public void ActivateLeftPower()
    {
        leftPower = true;
        CheckAndPlayAnimation();
    }

    public void ActivateRightPower()
    {
        rightPower = true;
        CheckAndPlayAnimation();
    }

    public void DeactivateLeftPower()
    {
        leftPower = false;
    }

    public void DeactivateRightPower()
    {
        rightPower = false;
    }

    private void CheckAndPlayAnimation()
    {
        if (leftPower && rightPower)
        {
            onBothPowerActivated?.Invoke();
            PlayAnimation();
        }
    }

    public void PlayAnimation()
    {
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }
}
