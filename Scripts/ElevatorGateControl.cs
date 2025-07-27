using UnityEngine;
using System.Collections;

public class ElevatorGateControl : MonoBehaviour
{
    public Transform leftObject; // Reference to the object to be moved left
    public Transform rightObject; // Reference to the object to be moved right
    public float moveAmount = 1.0f; // Amount to move the objects
    public float moveDuration = 1.0f; // Duration of the movement
    public float delayBeforeOpening = 0.5f; // Delay before starting the movement

    public void ActivateGate()
    {
        if (leftObject != null && rightObject != null)
        {
            StartCoroutine(MoveObjectsGradually());
        }
    }

    private IEnumerator MoveObjectsGradually()
    {
        // Wait for the specified delay before starting the movement
        yield return new WaitForSeconds(delayBeforeOpening);

        Vector3 leftStartPos = leftObject.position;
        Vector3 leftEndPos = leftObject.position + Vector3.left * moveAmount;
        Vector3 rightStartPos = rightObject.position;
        Vector3 rightEndPos = rightObject.position + Vector3.right * moveAmount;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            leftObject.position = Vector3.Lerp(leftStartPos, leftEndPos, elapsedTime / moveDuration);
            rightObject.position = Vector3.Lerp(rightStartPos, rightEndPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leftObject.position = leftEndPos;
        rightObject.position = rightEndPos;
    }
}