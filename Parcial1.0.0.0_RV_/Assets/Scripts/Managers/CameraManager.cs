using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    [Header("Jugadores")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Cámaras")]
    [SerializeField] private Camera sharedCamera;
    [SerializeField] private Camera player1Camera;
    [SerializeField] private Camera player2Camera;

    [Header("Player Controllers")]
    [SerializeField] private PlayerController player1Controller;
    [SerializeField] private PlayerController player2Controller;

    [Header("Pantalla dividida")]
    [SerializeField] private float splitDistance = 12f;

    [Header("Transicion")]
    [SerializeField] private float transitionSpeed = 2f;
    private float splitAmount = 0f;

    private SharedCameraController sharedController;

    private void Awake()
    {
        sharedController = sharedCamera.GetComponent<SharedCameraController>();
    }
    private void Update()
    {
        float distance = Vector3.Distance(player1.position, player2.position);

        // Decide si queremos pantalla dividida o no.
        float targetSplit = distance >= splitDistance ? 1f : 0f;

        // Anima la transición.
        splitAmount = Mathf.MoveTowards(
            splitAmount,
            targetSplit,
            transitionSpeed * Time.deltaTime
        );

        UpdateViewport();
    }
    private void UpdateViewport()
    {
        sharedCamera.gameObject.SetActive(splitAmount < 0.99f);

        player1Camera.gameObject.SetActive(splitAmount > 0.01f);
        player2Camera.gameObject.SetActive(splitAmount > 0.01f);

        if (splitAmount < 0.5f)
        {
            player1Controller.SetCameraTransform(sharedCamera.transform);
            player2Controller.SetCameraTransform(sharedCamera.transform);
        }
        else
        {
            player1Controller.SetCameraTransform(player1Camera.transform);
            player2Controller.SetCameraTransform(player2Camera.transform);
        }

        Rect leftRect = new Rect(
            0,
            0,
            1f - splitAmount * 0.5f,
            1
        );

        Rect rightRect = new Rect(
            0.5f + (0.5f - splitAmount * 0.5f),
            0,
            splitAmount * 0.5f,
            1
        );

        player1Camera.rect = leftRect;
        player2Camera.rect = rightRect;
    }
}