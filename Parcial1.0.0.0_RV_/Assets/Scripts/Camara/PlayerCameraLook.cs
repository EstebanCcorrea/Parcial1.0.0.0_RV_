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
    [Tooltip("Ahora se aplica directo al delta del mouse (sin Time.deltaTime). Valores típicos: 0.05 - 0.3")]
    [SerializeField] private float mouseSensitivity = 0.1f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minRadius = 2f;
    [SerializeField] private float maxRadius = 10f;

    [Tooltip("Transform del personaje (para alinear la cámara detrás de él al iniciar)")]
    [SerializeField] private Transform playerBody;

    private CinemachineOrbitalFollow orbitalFollow;
    private InputAction zoomAction;

    private void Awake()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        zoomAction = playerInput.actions["Zoom"];

        // Alinear la cámara detrás del personaje al iniciar, en vez de un ángulo
        // absoluto arbitrario del mundo (Binding Mode = World Space usa un
        // ángulo absoluto, no relativo a hacia dónde mira el personaje).
        if (playerBody != null)
        {
            orbitalFollow.HorizontalAxis.Value = playerBody.eulerAngles.y;
        }
    }

    private void Update()
    {
        Vector2 rawGamepad = ReadPairedGamepadInput();
        Vector2 rawMouse = ReadPairedMouseDelta();

        float beforeH = orbitalFollow.HorizontalAxis.Value;

        // Gamepad: valor continuo (-1..1) mientras el stick está inclinado -> escalar por Time.deltaTime.
        orbitalFollow.HorizontalAxis.Value += rawGamepad.x * horizontalSpeed * Time.deltaTime;
        orbitalFollow.VerticalAxis.Value -= rawGamepad.y * verticalSpeed * Time.deltaTime;

        // Mouse: el delta YA es "por frame", NO se multiplica por Time.deltaTime
        // (si se hace, en frames rápidos el giro se dispara y se ve como un salto/línea recta en vez de una órbita suave).
        orbitalFollow.HorizontalAxis.Value += rawMouse.x * mouseSensitivity;
        orbitalFollow.VerticalAxis.Value -= rawMouse.y * mouseSensitivity;

        // TEMP DEBUG: ver si el input horizontal llega y si el valor realmente cambia
        if (Mathf.Abs(rawGamepad.x) > 0.01f || Mathf.Abs(rawMouse.x) > 0.01f)
        {
            Debug.Log($"[{gameObject.name}] rawGamepad.x={rawGamepad.x:F3}, rawMouse.x={rawMouse.x:F3}, HorizontalAxis antes={beforeH:F2}, después={orbitalFollow.HorizontalAxis.Value:F2}");
        }

        ApplyZoom();
    }

    private Vector2 ReadPairedGamepadInput()
    {
        foreach (var device in playerInput.devices)
        {
            if (device is Gamepad gamepad)
            {
                Vector2 stick = gamepad.rightStick.ReadValue();
                if (stick.sqrMagnitude > 0.0001f) return stick;
            }
        }
        return Vector2.zero;
    }

    private Vector2 ReadPairedMouseDelta()
    {
        foreach (var device in playerInput.devices)
        {
            if (device is Mouse mouse)
            {
                return mouse.delta.ReadValue();
            }
        }
        return Vector2.zero;
    }

    private void ApplyZoom()
    {
        if (zoomAction == null) return;

        float zoomInput = zoomAction.ReadValue<float>();
        if (Mathf.Approximately(zoomInput, 0f)) return;

        float newRadius = orbitalFollow.Radius - zoomInput * zoomSpeed * Time.deltaTime;
        orbitalFollow.Radius = Mathf.Clamp(newRadius, minRadius, maxRadius);
    }

}