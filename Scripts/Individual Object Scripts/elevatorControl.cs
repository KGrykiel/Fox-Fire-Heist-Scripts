using System.Collections;
using UnityEngine;

namespace KinematicCharacterController.Examples
{
    public class elevatorControl : MonoBehaviour, IMoverController
    {
        public PhysicsMover Mover;
        public float targetHeight = 10f; // Target height to move the elevator
        public float moveDuration = 5f; // Duration of the movement

        private Vector3 _originalPosition;
        private Coroutine moveCoroutine;
        private bool hasMoved = false; // Flag to track if the elevator has already moved

        private void Start()
        {
            _originalPosition = Mover.Rigidbody.position;
            Mover.MoverController = this;
        }

        public void goUp()
        {
            if (!hasMoved)
            {
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }
                moveCoroutine = StartCoroutine(MoveElevatorUp());
                hasMoved = true; // Set the flag to true after starting the movement
            }
        }

        private IEnumerator MoveElevatorUp()
        {
            Vector3 startPosition = _originalPosition;
            Vector3 targetPosition = new Vector3(_originalPosition.x, _originalPosition.y + targetHeight, _originalPosition.z);
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                Mover.SetPosition(Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Mover.SetPosition(targetPosition);
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = Mover.Rigidbody.position;
            goalRotation = Mover.Rigidbody.rotation;
        }
    }
}
































