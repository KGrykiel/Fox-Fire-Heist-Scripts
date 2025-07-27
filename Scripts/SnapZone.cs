using UnityEngine;
using UnityEngine.Events;

public class SnapZone : MonoBehaviour
{
    public Transform snapTarget; // The target position and rotation for snapping
    public string requiredCustomTag = "Interactable"; // Custom tag to identify interactable objects
    public ObjectManipulator objectManipulator;
    public bool usePrefabMode = false; // Toggle between snapping and spawning prefab
    public GameObject prefabToSpawn; // Prefab to spawn at the target location
    public UnityEvent onObjectSnapped; // Event to propagate when an object is snapped
    public bool setKinematic = false; // Set the object to kinematic after snapping

    void OnTriggerEnter(Collider other)
    {
        ObjectProperties properties = other.GetComponent<ObjectProperties>();
        if (properties != null && properties.HasCustomTag(requiredCustomTag) && objectManipulator.heldObject == other.gameObject)
        {
            if (usePrefabMode)
            {
                SpawnPrefab(other.gameObject);
            }
            else
            {
                SnapObject(other.gameObject);
            }
        }
    }

    void SnapObject(GameObject obj)
    {
        // Move the object to the target position and rotation
        obj.transform.position = snapTarget.position;
        obj.transform.rotation = snapTarget.rotation;

        // Set the velocity of the object to zero
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            if (setKinematic)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        // Make the object usable (enable necessary components or scripts)
        MakeObjectUsable(obj);

        // Set the object to kinematic if required

        // Invoke the onObjectSnapped event
        onObjectSnapped.Invoke();
    }

    void SpawnPrefab(GameObject obj)
    {
        // Instantiate the prefab at the target position and rotation
        GameObject spawnedObject = Instantiate(prefabToSpawn, snapTarget.position, snapTarget.rotation);

        // Destroy the original object
        Destroy(obj);

        // Make the spawned object usable (enable necessary components or scripts)
        MakeObjectUsable(spawnedObject);

        // Invoke the onObjectSnapped event
        onObjectSnapped.Invoke();
    }

    void MakeObjectUsable(GameObject obj)
    {
        objectManipulator.DropObject(); // Drop the object if it is being held
        // Example: Enable a script or component to make the object usable
        // Replace this with your own logic as needed
        //var hingeJoint = obj.GetComponent<HingeJoint>();
        //if (hingeJoint != null)
        //{
        //    hingeJoint.enabled = true;
        //}
    }
}
