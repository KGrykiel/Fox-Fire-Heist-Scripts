using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Pressure_Gauge_2 : MonoBehaviour
{
    public Transform targetObject; // The object to rotate
    public float rotationDuration = 1f; // Duration of the rotation
    public UnityEvent onMaxPressureReached; // Event to propagate when maximum pressure is reached
    public ParticleSystem maxPressureParticleSystem; // Particle system to activate at maximum pressure

    private Quaternion[] targetRotations;
    private Coroutine rotationCoroutine;

    void Start()
    {
        // Define the four distinct rotations
        targetRotations = new Quaternion[4];
        targetRotations[0] = Quaternion.Euler(-57, 0, 0);
        targetRotations[1] = Quaternion.Euler(-77, 0, 0);
        targetRotations[2] = Quaternion.Euler(-102, 0, 0);
        targetRotations[3] = Quaternion.Euler(-123, 0, 0);
    }

    public void RotateToPosition(int positionIndex)
    {
        print("RotateToPosition called with index: " + positionIndex);
        if (positionIndex < 0 || positionIndex > 3)
        {
            return;
        }

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        rotationCoroutine = StartCoroutine(RotateObject(targetRotations[positionIndex]));

        if (positionIndex == 3)
        {
            print("Pressure gauge is at maximum pressure");
            onMaxPressureReached?.Invoke();
            ActivateMaxPressureParticleSystem();
        }
    }

    IEnumerator RotateObject(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion startingRotation = targetObject.localRotation;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            targetObject.localRotation = Quaternion.Slerp(startingRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.localRotation = targetRotation;
    }

    private void ActivateMaxPressureParticleSystem()
    {
        if (maxPressureParticleSystem != null)
        {
            maxPressureParticleSystem.Play();
        }
    }
}


