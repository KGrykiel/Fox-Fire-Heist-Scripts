using UnityEngine;
using System.Collections;

public class PressureGauge : MonoBehaviour
{
    public PipelineComponent pipelineComponent; // Reference to the PipelineComponent
    public Transform gaugeNeedle; // Reference to the child object to be rotated
    public float targetAngle = -240f; // The angle to rotate to when isFlowing is true
    public float lerpDuration = 1f; // Duration of the lerp
    public bool useSlerp = false; // Use Slerp instead of Lerp

    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private Coroutine lerpCoroutine;

    void Start()
    {
        if (pipelineComponent == null)
        {
            Debug.LogWarning("PipelineComponent is not assigned.");
            return;
        }

        if (gaugeNeedle == null)
        {
            Debug.LogWarning("GaugeNeedle is not assigned.");
            return;
        }

        originalRotation = gaugeNeedle.localRotation;
        targetRotation = Quaternion.Euler(targetAngle, 0f, 0f);

        // Subscribe to the onFlowChanged event
        pipelineComponent.onFlowChanged.AddListener(OnFlowChanged);
    }

    void OnDestroy()
    {
        // Unsubscribe from the onFlowChanged event
        if (pipelineComponent != null)
        {
            pipelineComponent.onFlowChanged.RemoveListener(OnFlowChanged);
        }
    }

    void OnFlowChanged(bool isFlowing)
    {
        // Stop any existing coroutine
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }

        // Start a new coroutine for the rotation
        lerpCoroutine = StartCoroutine(RotateObject(isFlowing ? targetRotation : originalRotation));
    }

    IEnumerator RotateObject(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion startingRotation = gaugeNeedle.localRotation;

        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;
            if (useSlerp)
            {
                gaugeNeedle.localRotation = QuaternionExtension.Slerp(startingRotation, targetRotation, t, false);
            }
            else
            {
                gaugeNeedle.localRotation = QuaternionExtension.Lerp(startingRotation, targetRotation, t, false);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gaugeNeedle.localRotation = targetRotation;
    }
}





