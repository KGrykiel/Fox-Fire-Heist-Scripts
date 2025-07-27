using UnityEngine;
using System.Collections;

public class HeaterController : MonoBehaviour
{
    public GameObject heaterObject; // Reference to the GameObject whose material will be changed
    public Material heaterMaterial; // Reference to the material
    public Light heaterLight; // Reference to the Light component
    [ColorUsage(true, true)]
    public Color onColor = new Color(1f, 0f, 0f); // Color when the heater is on
    public Color offColor = Color.white; // Color when the heater is off
    public float maxEmissionIntensity = 10.0f; // Maximum emission intensity
    public float maxLightIntensity = 100.0f; // Maximum light intensity
    public float lerpDuration = 1.0f; // Duration of the lerp

    private Coroutine lerpCoroutine;
    private Color initialEmissionColor;
    private float initialLightIntensity;

    void Start()
    {
        if (heaterObject == null)
        {
            Debug.LogWarning("Heater object not assigned on " + gameObject.name);
        }
        else
        {
            heaterMaterial = heaterObject.GetComponent<Renderer>().material;
            if (heaterMaterial == null)
            {
                Debug.LogWarning("Heater material not found on " + heaterObject.name);
            }
            else
            {
                initialEmissionColor = heaterMaterial.GetColor("_EmissionColor");
            }
        }

        if (heaterLight == null)
        {
            Debug.LogWarning("Heater light not assigned on " + gameObject.name);
        }
        else
        {
            initialLightIntensity = heaterLight.intensity;
        }
    }

    public void TurnOnHeater()
    {
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }
        lerpCoroutine = StartCoroutine(LerpEmissionColorAndLight(initialEmissionColor, onColor * maxEmissionIntensity, heaterMaterial.color, onColor, initialLightIntensity, maxLightIntensity));
    }

    public void TurnOffHeater()
    {
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }
        lerpCoroutine = StartCoroutine(LerpEmissionColorAndLight(heaterMaterial.GetColor("_EmissionColor"), initialEmissionColor, heaterMaterial.color, offColor, heaterLight.intensity, initialLightIntensity));
    }

    private IEnumerator LerpEmissionColorAndLight(Color startEmission, Color endEmission, Color startColor, Color endColor, float startIntensity, float endIntensity)
    {
        float elapsedTime = 0f;
        heaterMaterial.EnableKeyword("_EMISSION");
        while (elapsedTime < lerpDuration)
        {
            heaterMaterial.SetColor("_EmissionColor", Color.Lerp(startEmission, endEmission, elapsedTime / lerpDuration));
            heaterMaterial.color = Color.Lerp(startColor, endColor, elapsedTime / lerpDuration);
            if (heaterLight != null)
            {
                heaterLight.intensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / lerpDuration);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        heaterMaterial.SetColor("_EmissionColor", endEmission);
        heaterMaterial.color = endColor;
        if (heaterLight != null)
        {
            heaterLight.intensity = endIntensity;
        }
    }
}





















