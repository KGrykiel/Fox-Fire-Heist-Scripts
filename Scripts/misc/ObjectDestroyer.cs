using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public GameObject fracturedPrefab; // The fractured version of the object
    public float explosionForce = 5f; // The force applied to the fractured pieces
    public float explosionRadius = 1f; // The radius of the explosion force
    public float impactForceThreshold = 10f; // The threshold for the impact force to destroy the object

    private bool isDestroyed = false; // Flag to prevent multiple destructions

    void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed) return; // Prevent further processing if already destroyed

        Rigidbody collidingRb = collision.rigidbody;
        Rigidbody thisRb = GetComponent<Rigidbody>();

        if (thisRb != null)
        {
            // Calculate the relative velocity between the colliding objects
            Vector3 relativeVelocity = collision.relativeVelocity;
            // Calculate the angle between the relative velocity and the collision normal
            float angle = Vector3.Angle(relativeVelocity, collision.contacts[0].normal);
            // Calculate the impact force, adjusting for the angle of collision
            float impactForce = thisRb.mass * relativeVelocity.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
            //Sprint(impactForce);

            if (impactForce > impactForceThreshold)
            {
                DestroyObject();
                return;
            }
        }
        else if (collidingRb != null)
        {
            // Calculate the relative velocity between the colliding objects
            Vector3 relativeVelocity = collision.relativeVelocity;
            // Calculate the angle between the relative velocity and the collision normal
            float angle = Vector3.Angle(relativeVelocity, collision.contacts[0].normal);
            // Calculate the impact force using the mass of the colliding object, adjusting for the angle of collision
            float impactForce = collidingRb.mass * relativeVelocity.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
            //print(impactForce);

            if (impactForce > impactForceThreshold)
            {
                DestroyObject();
                return;
            }
        }
    }

    void DestroyObject()
    {
        if (isDestroyed) return; // Prevent multiple destructions
        isDestroyed = true; // Set the flag to indicate the object is destroyed

        // Instantiate the fractured prefab at the object's position and rotation
        GameObject fracturedObject = Instantiate(fracturedPrefab, transform.position, transform.rotation);
        fracturedObject.transform.localScale = transform.localScale;

        // Apply explosion force to the fractured pieces
        foreach (Rigidbody rb in fracturedObject.GetComponentsInChildren<Rigidbody>())
        {
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                //Debug.Log($"Applied explosion force to {rb.gameObject.name}");
            }
            else
            {
                Debug.LogWarning("No Rigidbody found on fractured piece.");
            }
        }

        // Destroy the original object
        Destroy(gameObject);
    }
}


