using UnityEngine;

public class ColorChangerEvent : MonoBehaviour
{
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer component found on this GameObject.");
        }
    }

    public void ChangeToRed()
    {
        ChangeColor(Color.red);
    }

    public void ChangeToGreen()
    {
        ChangeColor(Color.green);
    }

    public void ChangeToBlue()
    {
        ChangeColor(Color.blue);
    }

    private void ChangeColor(Color newColor)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = newColor;
        }
    }
}


