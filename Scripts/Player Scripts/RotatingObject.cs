using UnityEngine;
using System.Collections;

public class RotatingObject : MonoBehaviour
{
    public float rotationSpeed = 300f; // Maximum rotation speed in degrees per second
    public float lerpDuration = 1f; // Duration of the lerp
    public Vector3 rotationAxis = Vector3.up; // Axis of rotation
    private float currentSpeed = 0f; // Current rotation speed
    private Coroutine lerpCoroutine;

    void Update()
    {
        if (currentSpeed > 0f)
        {
            // Rotate the object around the specified axis
            transform.Rotate(rotationAxis, currentSpeed * Time.deltaTime);
        }
    }

    public void TurnOn()
    {
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }
        lerpCoroutine = StartCoroutine(LerpRotationSpeed(rotationSpeed));
    }

    public void TurnOff()
    {
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }
        lerpCoroutine = StartCoroutine(LerpRotationSpeed(0f));
    }

    private IEnumerator LerpRotationSpeed(float targetSpeed)
    {
        float startSpeed = currentSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;
            currentSpeed = Mathf.SmoothStep(startSpeed, targetSpeed, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentSpeed = targetSpeed;
    }
}









