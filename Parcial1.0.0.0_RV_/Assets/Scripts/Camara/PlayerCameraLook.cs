using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

/// <summary>
/// Controla la cámara orbital leyendo el right stick / mouse SOLO de los
/// dispositivos ya emparejados a este PlayerInput específico (via
/// InputManager), evitando que un stick "genérico" controle ambas cámaras.
/// </summary>
[RequireComponent(typeof(CinemachineOrbitalFollow))]
public class PlayerCameraLook : MonoBehaviour
{
    [Tooltip("El PlayerInput de ESTE jugador (Soldado o Mago)")]
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float horizontalSpeed = 200f;
    [SerializeField] private float verticalSpeed = 130f;
    [SerializeField] private float mouseSensitivity = 0.5f;

    private CinemachineOrbitalFollow orbitalFollow;

    private void Awake()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    private void Update()
    {
        Vector2 raw = ReadPairedLookInput();

        orbitalFollow.HorizontalAxis.Value += raw.x * horizontalSpeed * Time.deltaTime;
        orbitalFollow.VerticalAxis.Value -= raw.y * verticalSpeed * Time.deltaTime;
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