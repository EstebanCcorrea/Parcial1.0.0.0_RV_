using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

/// <summary>
/// Controla la cámara orbital leyendo el right stick / mouse SOLO de los
/// dispositivos ya emparejados a este PlayerInput específico (via
/// InputManager), evitando que un stick "genérico" controle ambas cámaras.
/// También controla el zoom (Radius de CinemachineOrbitalFollow) usando
/// la acción "Zoom" (gatillos en gamepad / rueda del mouse en teclado).
/// </summary>
[RequireComponent(typeof(CinemachineOrbitalFollow))]
public class PlayerCameraLook : MonoBehaviour
{
    [Tooltip("El PlayerInput de ESTE jugador (Soldado o Mago)")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Rotación")]
    [SerializeField] private float horizontalSpeed = 200f;
    [SerializeField] private float verticalSpeed = 130f;
    [SerializeField] private float mouseSensitivity = 0.5f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minRadius = 2f;
    [SerializeField] private float maxRadius = 10f;

    private CinemachineOrbitalFollow orbitalFollow;
    private InputAction zoomAction;

    private void Awake()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        zoomAction = playerInput.actions["Zoom"];
    }

    private void Update()
    {
        Vector2 raw = ReadPairedLookInput();

        orbitalFollow.HorizontalAxis.Value += raw.x * horizontalSpeed * Time.deltaTime;
        orbitalFollow.VerticalAxis.Value -= raw.y * verticalSpeed * Time.deltaTime;

        ApplyZoom();
    }

    private void ApplyZoom()
    {
        if (zoomAction == null) return;

        float zoomInput = zoomAction.ReadValue<float>();
        if (Mathf.Approximately(zoomInput, 0f)) return;

        float newRadius = orbitalFollow.Radius - zoomInput * zoomSpeed * Time.deltaTime;
        orbitalFollow.Radius = Mathf.Clamp(newRadius, minRadius, maxRadius);
    }

    private Vector2 ReadPairedLookInput()
    {
        foreach (var device in playerInput.devices)
        {
            if (device is Gamepad gamepad)
            {
                Vector2 stick = gamepad.rightStick.ReadValue();
                if (stick.sqrMagnitude > 0.0001f) return stick;
            }
            else if (device is Mouse mouse)
            {
                Vector2 delta = mouse.delta.ReadValue() * mouseSensitivity;
                if (delta.sqrMagnitude > 0.0001f) return delta;
            }
        }
        return Vector2.zero;
    }
}