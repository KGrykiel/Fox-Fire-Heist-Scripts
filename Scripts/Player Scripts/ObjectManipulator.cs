using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectManipulator : MonoBehaviour
{
    public Camera playerCamera;
    public float pickupRange = 3f;
    public float holdDistance = 2f;
    public float throwForce = 10f;
    public float maxAngle = 45f; // Maximum angle in degrees before dropping the object
    public float maxDistance = 5f; // Maximum distance before dropping the object
    public float rotationSpeed = 500f; // Speed of rotation when holding the "R" key
    public float scrollSpeed = 10f; // Speed of adjusting the hold distance with the scroll wheel
    public CrosshairManager crosshairManager; // Reference to the CrosshairManager
    public GameObject heldObject = null;
    public Collider playerFloorCollider;
    private Rigidbody heldObjectRb = null;
    private float initialDrag = 0;
    private float initialAngularDrag = 0;

    // Input flags and values
    private bool throwInput = false;
    private bool canPickup = true; // Flag to track if the object can be picked up
    private float scrollInput = 0f;
    private Vector2 mouseDelta;

    void Update()
    {
        if (PauseMenu.isPaused || PauseMenu.isStart) return; // Disable interactions when the game is paused

        // Detect input
        if (Input.GetMouseButtonDown(0))
        {
            TryPickupObject();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (heldObject != null)
            {
                DropObject();
            }
            canPickup = true; // Allow picking up again after releasing LMB
        }

        if (Input.GetMouseButtonDown(1) && heldObject)
        {
            throwInput = true;
        }

        scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Capture mouse movement only if 'R' key is pressed
        if (Input.GetKey(KeyCode.R))
        {
            mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        else
        {
            mouseDelta = Vector2.zero;
        }

        UpdateCrosshair();
        if (heldObject != null)
        {
            AdjustHoldDistance();
        }
    }

    void FixedUpdate()
    {
        // Handle throw input
        if (throwInput && heldObject != null)
        {
            ThrowObject();
            throwInput = false;
            canPickup = false; // Prevent picking up again until LMB is released
        }

        // Handle physics updates
        if (heldObject != null)
        {
            if (ShouldDropObject())
            {
                DropObject();
            }
            else
            {
                MoveObject();
                RotateObject();
            }
        }
    }

    void TryPickupObject()
    {
        if (!canPickup) return; // Prevent picking up if not allowed

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.red, 1f);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.gameObject.CompareTag("Interactable"))
            {
                if(playerFloorCollider.bounds.Intersects(hit.collider.bounds))
                {
                    return;
                }
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();
                if (heldObjectRb != null)
                {
                    initialDrag = heldObjectRb.drag; // Store the initial drag value
                    initialAngularDrag = heldObjectRb.angularDrag;

                    // Check for custom drag value
                    ObjectProperties properties = heldObject.GetComponent<ObjectProperties>();
                    if (properties != null && properties.customDrag >= 0)
                    {
                        heldObjectRb.drag = properties.customDrag;
                    }
                    else
                    {
                        heldObjectRb.drag = Mathf.Clamp(10 / heldObjectRb.mass, 3, 7);
                    }

                    heldObjectRb.useGravity = false;
                    heldObjectRb.angularDrag = 10;
                    heldObjectRb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Set collision detection mode

                    // Calculate the hold distance
                    Vector3 manipulationOffset = GetManipulationOffset();
                    Vector3 objectCenter = heldObject.transform.position + manipulationOffset;
                    holdDistance = Vector3.Distance(playerCamera.transform.position, objectCenter);
                }
            }
        }
    }

    public void DropObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = true;
            heldObjectRb.drag = initialDrag; // Restore the initial drag value
            heldObjectRb.angularDrag = initialAngularDrag;
            heldObjectRb.collisionDetectionMode = CollisionDetectionMode.Discrete; // Restore collision detection mode
        }
        heldObject = null;
        heldObjectRb = null;
        resetHoldDistance();
    }
    public float proportionalGain = 200f;
    public float derivativeGain = 25f;
    void MoveObject()
    {
        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
        Vector3 currentPosition = heldObject.transform.position + GetManipulationOffset();
        Vector3 direction = targetPosition - currentPosition;
        float distance = direction.magnitude; // Calculate the distance to the target position

        // Proportional term
        //float proportionalGain = 200f; // Adjust this value to control the responsiveness
        Vector3 proportionalForce = direction.normalized * distance * proportionalGain;

        // Derivative term
        //float derivativeGain = 25f; // Adjust this value to control the damping
        Vector3 velocity = heldObjectRb.velocity;
        Vector3 derivativeForce = -velocity * derivativeGain;

        // Calculate the total force
        Vector3 totalForce = proportionalForce + derivativeForce;

        // Apply the force
        heldObjectRb.AddForce(totalForce, ForceMode.Force);
    }

    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R) && mouseDelta != Vector2.zero)
        {
            // Clamp mouseDelta to a reasonable range
            float clampedMouseDeltaX = Mathf.Clamp(mouseDelta.x, -3f, 3f);
            float clampedMouseDeltaY = Mathf.Clamp(mouseDelta.y, -3f, 3f);

            if (heldObject.GetComponent<ObjectProperties>() != null && heldObject.GetComponent<ObjectProperties>().disableRotation)
            {
                return;
            }

            if(heldObjectRb != null && heldObjectRb.isKinematic)
            {
                return;
            } 

            Vector3 manipulationOffset = GetManipulationOffset();
            Vector3 pivotPoint = heldObject.transform.position + manipulationOffset;

            HingeJoint hinge = heldObject.GetComponent<HingeJoint>();
            if (hinge != null)
            {
                // Rotate around the hinge's axis
                float rotationAmount = clampedMouseDeltaX * rotationSpeed * Time.fixedDeltaTime;
                heldObject.transform.Rotate(hinge.axis, rotationAmount, Space.Self);
            }
            else
            {
                // Rotate freely
                float rotationAmountY = clampedMouseDeltaX * rotationSpeed * Time.fixedDeltaTime;
                float rotationAmountX = -clampedMouseDeltaY * rotationSpeed * Time.fixedDeltaTime;

                heldObject.transform.RotateAround(pivotPoint, Vector3.up, rotationAmountY);
                heldObject.transform.RotateAround(pivotPoint, Vector3.right, rotationAmountX);
            }
        }
    }

    void AdjustHoldDistance()
    {
        holdDistance += scrollInput * scrollSpeed;
        holdDistance = Mathf.Clamp(holdDistance, 1.3f, maxDistance - 1); // Clamp the hold distance to a reasonable range
    }

    void ThrowObject()
    {
        if (heldObjectRb != null)
        {
            heldObjectRb.useGravity = true;
            heldObjectRb.drag = initialDrag; // Restore the initial drag value
            heldObjectRb.angularDrag = initialAngularDrag;
            heldObjectRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Set collision detection mode for throwing
            heldObjectRb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
        }
        heldObject = null;
        heldObjectRb = null;
        resetHoldDistance();
    }

    bool ShouldDropObject()
    {
        // Check if the distance between the player and the object is too large
        float distance = Vector3.Distance(playerCamera.transform.position, heldObject.transform.position);
        if (distance > maxDistance)
        {
            return true;
        }

        return false;
    }

    void resetHoldDistance()
    {
        holdDistance = 2f;
    }

    void UpdateCrosshair()
    {
        if (heldObject != null)
        {
            crosshairManager.SetHoldingCrosshair();
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.GetComponent<Note>() != null)
            {
                crosshairManager.SetNoteCrosshair();
            }
            else if (hit.collider.gameObject.CompareTag("Interactable") && !playerFloorCollider.bounds.Intersects(hit.collider.bounds))
            {
                crosshairManager.SetInteractableCrosshair();
            }
            else
            {
                crosshairManager.SetDefaultCrosshair();
            }
        }
        else
        {
            crosshairManager.SetDefaultCrosshair();
        }
    }

    Vector3 GetManipulationOffset()
    {
        ObjectProperties properties = heldObject.GetComponent<ObjectProperties>();
        if (properties != null)
        {
            return heldObject.transform.TransformPoint(properties.ManipulationOffset) - heldObject.transform.position;
        }
        return Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody == heldObjectRb)
        {
            DropObject();
        }

    }
}
