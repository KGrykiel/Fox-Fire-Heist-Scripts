using UnityEngine;
using System.Collections.Generic;

public class ObjectProperties : MonoBehaviour
{
    public bool coal = false; // Property to indicate if the object is coal
    public bool wheel = false; // Property to indicate if the object is a wheel
    public bool disableRotation = false; // Property to disable rotation when holding the object
    public float customDrag = -1; // Custom drag value for the object
    public List<string> customTags = new List<string>(); // List of custom tags

    [SerializeField]
    private Vector3 manipulationOffset = Vector3.zero; // Offset of the "center" of the object for manipulation

    // Method to add a custom tag
    public void AddCustomTag(string tag)
    {
        if (!customTags.Contains(tag))
        {
            customTags.Add(tag);
        }
    }

    // Method to remove a custom tag
    public void RemoveCustomTag(string tag)
    {
        if (customTags.Contains(tag))
        {
            customTags.Remove(tag);
        }
    }

    // Method to check if a custom tag exists
    public bool HasCustomTag(string tag)
    {
        return customTags.Contains(tag);
    }

    // Property to get and set the manipulation offset
    public Vector3 ManipulationOffset
    {
        get { return manipulationOffset; }
        set { manipulationOffset = value; }
    }

    // Draw a gizmo to visualize the manipulation offset in the scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(manipulationOffset), 0.01f);
    }
}



