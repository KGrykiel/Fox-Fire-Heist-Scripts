using UnityEngine;

public class RigidbodyKinematicController : MonoBehaviour
{
    public Rigidbody targetRigidbody; // Reference to the Rigidbody component

    void Start()
    {
        if (targetRigidbody == null)
        {
            targetRigidbody = GetComponent<Rigidbody>();
            if (targetRigidbody == null)
            {
                Debug.LogWarning("Rigidbody component not found on " + gameObject.name);
            }
        }
    }

    public void SetKinematic(bool isKinematic)
    {
        if (targetRigidbody != null)
        {
            targetRigidbody.isKinematic = isKinematic;
        }
    }
}
