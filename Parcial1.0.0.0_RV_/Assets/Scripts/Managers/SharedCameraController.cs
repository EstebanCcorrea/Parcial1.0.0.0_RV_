using UnityEngine;

public class SharedCameraController : MonoBehaviour
{
    [Header("Jugadores")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Posición de cámara")]
    [SerializeField] private Vector3 offset = new Vector3(0, 8, -10);

    [Header("Suavizado")]
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        Vector3 center = (player1.position + player2.position) / 2f;

        Vector3 targetPosition = center + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(center);
    }
}