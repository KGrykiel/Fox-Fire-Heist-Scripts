using UnityEngine;
using UnityEngine.Events;

public class GeneratorController : MonoBehaviour
{
    public bool power = false; // Power state
    public UnityEvent onLeverPulled; // Event to propagate when the lever is pulled
    public Renderer targetRenderer; // Renderer of the object whose material will be changed
    public Color emissiveColor = Color.red; // Emissive color when the lever is pulled
    public float emissiveIntensity = 1.0f; // Emissive intensity
    public Color materialColor = Color.red; // Color of the material when the lever is pulled

    public void powerOn()
    {
        power = true;
    }

    public void powerOff()
    {
        power = false;
    }

    public void leverPulled()
    {
        if (power)
        {
            onLeverPulled?.Invoke();
            SetEmissiveColor(targetRenderer, emissiveColor, emissiveIntensity, materialColor);
        }
    }

    private void SetEmissiveColor(Renderer renderer, Color emissiveColor, float intensity, Color mainColor)
    {
        if (renderer != null)
        {
            Material material = renderer.material;
            Color finalEmissiveColor = emissiveColor * Mathf.LinearToGammaSpace(intensity);
            material.SetColor("_EmissionColor", finalEmissiveColor);
            material.color = mainColor;
            material.EnableKeyword("_EMISSION");
            DynamicGI.SetEmissive(renderer, finalEmissiveColor);
        }
    }
}