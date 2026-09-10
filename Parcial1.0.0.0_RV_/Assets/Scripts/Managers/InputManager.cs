using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

/// <summary>
/// Empareja manualmente los dispositivos compartidos (teclado) y los gamepads
/// con cada PlayerInput, siguiendo el patrón oficial de Unity para
/// "left/right keyboard splits" en multijugador local.
/// Docs: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/UserManagement.html
/// </summary>
public class InputManager : MonoBehaviour
{
    [Header("Player Inputs")]
    [SerializeField] private PlayerInput soldadoInput;
    [SerializeField] private PlayerInput magoInput;

    private void Start()
    {
        // Empezamos limpio: quitamos cualquier emparejamiento automático
        // que el PlayerInput haya hecho solo al activarse (esto es lo que
        // suele "robarle" el teclado a uno de los dos).
        ClearPairings(soldadoInput);
        ClearPairings(magoInput);

        PairKeyboard();
        PairGamepads();

        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void ClearPairings(PlayerInput input)
    {
        var devices = new List<InputDevice>(input.user.pairedDevices);
        foreach (var device in devices)
        {
            input.user.UnpairDevice(device);
        }
    }

    private void PairKeyboard()
    {
        if (Keyboard.current == null) return;

        // Mismo teclado, dos usuarios. Cada Action Map (Player1/Player2)
        // ya filtra por bindings distintos (WASD vs Flechas), así que no
        // hay conflicto aunque el dispositivo físico sea el mismo.
        InputUser.PerformPairingWithDevice(Keyboard.current, soldadoInput.user);
        InputUser.PerformPairingWithDevice(Keyboard.current, magoInput.user);
    }

    private void PairGamepads()
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count > 0)
            InputUser.PerformPairingWithDevice(gamepads[0], soldadoInput.user);

        if (gamepads.Count > 1)
            InputUser.PerformPairingWithDevice(gamepads[1], magoInput.user);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change != InputDeviceChange.Added) return;
        if (device is not Gamepad gamepad) return;

        // Un gamepad nuevo se asigna al primer jugador que aún no tenga uno.
        if (!Contains(soldadoInput.user, gamepad))
            InputUser.PerformPairingWithDevice(gamepad, soldadoInput.user);
        else if (!Contains(magoInput.user, gamepad))
            InputUser.PerformPairingWithDevice(gamepad, magoInput.user);
    }

    private bool Contains(InputUser user, InputDevice device)
    {
        foreach (var d in user.pairedDevices)
            if (d == device) return true;
        return false;
    }
}