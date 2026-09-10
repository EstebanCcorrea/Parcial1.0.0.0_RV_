using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// Cambia el Control Scheme activo detectando actividad a nivel de sistema
/// (InputSystem.onEvent), independiente del "Behavior" configurado en el
/// PlayerInput. Necesario porque onActionTriggered solo funciona con
/// Behavior = Invoke C Sharp Events.
/// Docs: https://docs.unity3d.com/Packages/com.unity.inputsystem/api/UnityEngine.InputSystem.PlayerInput.html
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class ManualSchemeSwitcher : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.neverAutoSwitchControlSchemes = true;
    }

    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        // Solo nos interesan dispositivos ya emparejados a ESTE jugador.
        if (!IsPairedToThisPlayer(device)) return;

        var scheme = InputControlScheme.FindControlSchemeForDevice(
            device,
            playerInput.actions.controlSchemes
        );

        if (scheme.HasValue && scheme.Value.name != playerInput.currentControlScheme)
        {
            playerInput.SwitchCurrentControlScheme(scheme.Value.name, device);
        }
    }

    private bool IsPairedToThisPlayer(InputDevice device)
    {
        foreach (var d in playerInput.devices)
        {
            if (d == device) return true;
        }
        return false;
    }
}