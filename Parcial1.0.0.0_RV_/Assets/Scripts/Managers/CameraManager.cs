using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Jugadores")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Cámaras")]
    [SerializeField] private Camera sharedCamera;
    [SerializeField] private Camera player1Camera;
    [SerializeField] private Camera player2Camera;

    [Header("Pantalla dividida")]
    [SerializeField] private float splitDistance = 12f;

    private SharedCameraController sharedController;

    private void Awake()
    {
        sharedController = sharedCamera.GetComponent<SharedCameraController>();
    }
    private void Update()
    {
        float distance = Vector3.Distance(player1.position, player2.position);

        Debug.Log("Distancia: " + distance);

        if (distance >= splitDistance)
        {
            Debug.Log("SPLIT SCREEN ACTIVADO");
            EnableSplitScreen();
        }
        else
        {
            Debug.Log("CAMARA COMPARTIDA");
            EnableSharedCamera();
        }
    }

    private void EnableSharedCamera()
    {
        sharedCamera.gameObject.SetActive(true);

        player1Camera.gameObject.SetActive(false);
        player2Camera.gameObject.SetActive(false);
    }

    private void EnableSplitScreen()
    {
        sharedCamera.gameObject.SetActive(false);

        player1Camera.gameObject.SetActive(true);
        player2Camera.gameObject.SetActive(true);
    }

}