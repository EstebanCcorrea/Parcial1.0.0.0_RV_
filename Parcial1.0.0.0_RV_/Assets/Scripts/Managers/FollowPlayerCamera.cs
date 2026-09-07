using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 8, -10);
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(target.position);
    }
}