using UnityEngine;
using UnityEngine.InputSystem;

public class FollowPlayerCamera : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform target;

    [Header("Distancia")]
    [SerializeField] private float distance = 8f;
    [SerializeField] private float height = 3f;

    [Header("Rotación")]
    [SerializeField] private float sensitivity = 120f;
    [SerializeField] private float smoothSpeed = 8f;

    private Vector2 lookInput;

    private float yaw;
    private float pitch = 15f;

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        yaw += lookInput.x * sensitivity * Time.deltaTime;
        pitch -= lookInput.y * sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -20f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 offset = rotation * new Vector3(0, height, -distance);

        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}