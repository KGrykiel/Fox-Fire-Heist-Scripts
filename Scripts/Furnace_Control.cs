using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Furnace_Control : MonoBehaviour
{
    public ParticleSystem embersVFX; // Reference to the particle system

    private int specificObjectCount = 0;

    // Define the event
    public UnityEvent<int> onObjectCountChanged = new UnityEvent<int>();

    void OnTriggerEnter(Collider other)
    {
        ObjectProperties properties = other.GetComponent<ObjectProperties>();
        if (properties != null && properties.coal)
        {
            // Play the particle effect at the object's position
            if (embersVFX != null)
            {
                embersVFX.Play();
            }

            // Destroy the object
            Destroy(other.gameObject);
            specificObjectCount += 1;
            PropagateObjectCount();
        }
    }

    void PropagateObjectCount()
    {
        // Invoke the event with the current object count
        onObjectCountChanged.Invoke(specificObjectCount);
    }
}


