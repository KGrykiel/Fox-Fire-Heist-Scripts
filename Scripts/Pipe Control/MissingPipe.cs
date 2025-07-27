using UnityEngine;

public class MissingPipe : PipelineComponent
{
    public string requiredTag = "ReplacementPipe"; // The custom tag to check for
    public MeshRenderer meshRenderer; // Reference to the MeshRenderer component
    public SphereCollider sphereCollider; // Reference to the SphereCollider component
    public MeshCollider meshCollider; // Reference to the MeshCollider component

    private bool filled = false; // Whether the pipe is filled with a replacement pipe

    void Start()
    {
        // Ensure the MeshRenderer, SphereCollider, and MeshCollider are assigned
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        if (sphereCollider == null)
        {
            sphereCollider = GetComponent<SphereCollider>();
        }
        if (meshCollider == null)
        {
            meshCollider = GetComponent<MeshCollider>();
        }

        // Initially, the MeshRenderer should be off, SphereCollider on, and MeshCollider off
        meshRenderer.enabled = false;
        sphereCollider.enabled = true;
        meshCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the required custom tag
        ObjectProperties objectProperties = other.GetComponent<ObjectProperties>();
        if (objectProperties != null && objectProperties.HasCustomTag(requiredTag))
        {
            // Delete the object
            Destroy(other.gameObject);

            // Enable the MeshRenderer
            meshRenderer.enabled = true;

            // Disable the SphereCollider
            sphereCollider.enabled = false;

            // Enable the MeshCollider
            meshCollider.enabled = true;

            // Set the filled flag to true
            filled = true;

            // Unblock the flow
            SetFlow(isFlowing);
        }
    }

    public override void SetFlow(bool isFlowing)
    {
        this.isFlowing = isFlowing; // Flow is always blocked by a missing pipe

        if (filled)
        {
            ChangeColor(isFlowing);
        }

        if (isFlowing && !filled)
        {
            // start a particle system
            print("imagine steam coming out rn");
        }

        if (nextComponent != null)
        {
            nextComponent.SetFlow(isFlowing && filled);
        }

    }
}




