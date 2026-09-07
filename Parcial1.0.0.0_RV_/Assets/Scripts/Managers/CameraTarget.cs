using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    private void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        transform.position = (player1.position + player2.position) * 0.5f;
    }
}