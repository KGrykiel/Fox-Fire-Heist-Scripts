using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FoxFireController : MonoBehaviour
{
    public Rigidbody targetRigidbody; // Reference to the Rigidbody component
    public Light[] lightsToDisable; // List of lights to disable
    public ReflectionProbe reflectionProbe; // Reference to the ReflectionProbe component
    public ParticleSystem[] particleSystems; // List of particle systems to activate and deactivate
    public ParticleSystem[] particleSystems2;
    public UnityEvent onBothPowerActivated; // Event to propagate when both conditions are true
    public float actionDelay = 1f; // Delay before performing actions
    public float particleEffectDuration = 2f; // Duration to keep particle effects active

    private bool leftPower = false;
    private bool rightPower = false;

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

        if (reflectionProbe == null)
        {
            reflectionProbe = GetComponent<ReflectionProbe>();
            if (reflectionProbe == null)
            {
                Debug.LogWarning("ReflectionProbe component not found on " + gameObject.name);
            }
        }
    }

    public void ActivateLeftPower()
    {
        leftPower = true;
        CheckAndPerformActions();
    }

    public void ActivateRightPower()
    {
        rightPower = true;
        CheckAndPerformActions();
    }

    public void DeactivateLeftPower()
    {
        leftPower = false;
    }

    public void DeactivateRightPower()
    {
        rightPower = false;
    }

    private void CheckAndPerformActions()
    {
        if (leftPower && rightPower)
        {
            StartCoroutine(PerformActionsWithDelay());
        }
    }

    private IEnumerator PerformActionsWithDelay()
    {
        yield return new WaitForSeconds(2);
        // Activate particle systems

        onBothPowerActivated?.Invoke();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            if (particleSystem != null)
            {
                yield return new WaitForSeconds(Random.Range(0.4f,1.0f));
                particleSystem.Play();
            }
        }
        yield return new WaitForSeconds(actionDelay);

        // Change the tag of the object
        gameObject.tag = "Interactable";

        // Change the Rigidbody to not dynamic (set to kinematic)
        if (targetRigidbody != null)
        {
            targetRigidbody.isKinematic = false;
            targetRigidbody.AddForce(Vector3.forward * 3, ForceMode.Impulse);
        }

        // Disable the list of lights
        foreach (Light light in lightsToDisable)
        {
            if (light != null)
            {
                light.enabled = false;
            }
        }

        // Reset the reflection probe
        if (reflectionProbe != null)
        {
            reflectionProbe.enabled = false;
        }

        // Wait for the particle effect duration
        yield return new WaitForSeconds(particleEffectDuration);

        // Deactivate particle systems
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }

        // Activate the second set of particle systems
        foreach (ParticleSystem particleSystem in particleSystems2)
        {
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }
    }
}

