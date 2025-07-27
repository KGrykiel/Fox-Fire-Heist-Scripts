using UnityEngine;

public class StarterBoxHatchController : MonoBehaviour
{
    public Transform rotationSource; // The object whose rotation will control the movement
    public float maxMovementDistance = 5f; // Maximum distance the object can move
    public float rotationThreshold = 90f; // Rotation in degrees to reach maximum movement

    private bool isEnabled = true; // Flag to enable/disable the functionality
    private Vector3 initialPosition; // Initial position of the object

    void Start()
    {
        if (rotationSource == null)
        {
            Debug.LogWarning("Rotation source is not assigned.");
            enabled = false;
            return;
        }

        initialPosition = transform.position;
    }

    void Update()
    {
        if (!isEnabled) return;

        // Calculate the rotation angle around the Y axis
        float rotationAngle = rotationSource.localEulerAngles.y;
        if (rotationAngle > 180f) rotationAngle -= 360f; // Normalize the angle to the range [-180, 180]

        // Calculate the movement distance based on the rotation angle
        float movementDistance = Mathf.Clamp(rotationAngle / rotationThreshold, -1f, 1f) * maxMovementDistance;

        // Move the object left or right based on the rotation angle
        Vector3 targetPosition = initialPosition + transform.right * movementDistance;
        transform.position = targetPosition;
    }

    // Public method to disable the functionality
    public void DisableMovement()
    {
        isEnabled = false;
    }
}

