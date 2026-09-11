using UnityEngine;

public class SharedCameraController : MonoBehaviour
{
    [Header("Jugadores")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Posición de la cámara")]
    [SerializeField] private Vector3 offset = new Vector3(0, 8, -10);

    [Header("Zoom")]
    [SerializeField] private float minDistance = 8f;
    [SerializeField] private float maxDistance = 18f;
    [SerializeField] private float maxPlayerDistance = 15f;

    [Header("Movimiento suave")]
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        Vector3 center = (player1.position + player2.position) / 2f;

        float playersDistance = Vector3.Distance(player1.position, player2.position);

        float zoom = Mathf.Lerp(minDistance, maxDistance, playersDistance / maxPlayerDistance);

        Vector3 targetPosition = center + new Vector3(0, zoom, -zoom);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(center);

    }
}