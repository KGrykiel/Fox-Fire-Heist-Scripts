using UnityEngine;

public class EnableObjectOnTrigger : MonoBehaviour
{
    public string requiredTag = "Interactable"; // The custom tag to check for
    public GameObject objectToEnable; // The object to enable when the trigger is hit
    public Collider Collider; // Reference to the SphereCollider component

    private bool isEnabled = false; // Whether the object has been enabled

    void Start()
    {
        // Initially, the object to enable should be inactive
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the required custom tag
        ObjectProperties objectProperties = other.GetComponent<ObjectProperties>();
        if (objectProperties != null && objectProperties.HasCustomTag(requiredTag))
        {
            // Enable the object
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
            }

            // Disable the Collider
            Collider.enabled = false;

            // Set the isEnabled flag to true
            isEnabled = true;

            //delete the other object
            Destroy(other.gameObject);
        }
    }

}
