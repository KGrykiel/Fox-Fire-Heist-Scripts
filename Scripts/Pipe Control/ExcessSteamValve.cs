using UnityEngine;

public class ExcessSteamValve : PipelineComponent
{
    public ParticleSystem steamEffect;
    public Collider steamTriggerCollider; // Specific trigger type collider for steam

    public float steamForce = 10f; // Force applied by the steam
    public Vector3 forceDirection = Vector3.up; // Direction of the force

    private void Start()
    {
        if (steamTriggerCollider == null)
        {
            Debug.LogWarning("Steam trigger collider not assigned on " + gameObject.name);
        }
        else
        {
            steamTriggerCollider.isTrigger = true;
        }
    }

    public override void SetFlow(bool isFlowing)
    {
        base.SetFlow(isFlowing);
        if (steamEffect != null)
        {
            if (isFlowing)
            {
                steamEffect.Play();
                ApplySteamForceToNearbyObjects();
            }
            else
            {
                steamEffect.Stop();
            }
        }
    }

    private void ApplySteamForceToNearbyObjects()
    {
        if (steamTriggerCollider != null)
        {
            Collider[] colliders = Physics.OverlapBox(steamTriggerCollider.bounds.center, steamTriggerCollider.bounds.extents, steamTriggerCollider.transform.rotation);
            foreach (Collider collider in colliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    ApplySteamForce(rb);
                }
            }
        }
    }

    private void ApplySteamForce(Rigidbody rb)
    {
        Vector3 force = forceDirection.normalized * steamForce;
        rb.AddForce(force, ForceMode.Impulse);
    }
}

















