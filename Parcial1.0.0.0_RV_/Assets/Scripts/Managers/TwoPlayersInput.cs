using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class TwoPlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerInput soldado;
    [SerializeField] private PlayerInput mago;

    private void Start()
    {
        ConfigurarControles();
    }

    private void ConfigurarControles()
    {
        if (soldado == null || mago == null)
        {
            Debug.LogError("Falta asignar Soldado o Mago.");
            return;
        }

        if (Gamepad.all.Count < 2)
        {
            Debug.LogError(
                "Se necesitan 2 gamepads. Detectados: "
                + Gamepad.all.Count
            );
            return;
        }

        Gamepad gamepadSoldado = null;
        Gamepad gamepadMago = null;

        foreach (Gamepad gamepad in Gamepad.all)
        {
            Debug.Log("Gamepad detectado: " + gamepad.displayName);

            if (gamepad.displayName.Contains("DualShock"))
            {
                gamepadSoldado = gamepad;
            }
            else if (
                gamepad.displayName.Contains("XInput") ||
                gamepad.displayName.Contains("Xbox")
            )
            {
                gamepadMago = gamepad;
            }
        }

        if (gamepadSoldado == null || gamepadMago == null)
        {
            Debug.LogError(
                "No se pudieron identificar los dos gamepads."
            );
            return;
        }

        // Desemparejar dispositivos anteriores
        soldado.user.UnpairDevices();
        mago.user.UnpairDevices();

        // Teclado
        Keyboard teclado = Keyboard.current;

        if (teclado == null)
        {
            Debug.LogError("No se detectó el teclado.");
            return;
        }

        // SOLDADO
        soldado.SwitchCurrentControlScheme(
            "keyboard+controller",
            teclado,
            gamepadSoldado
        );

        // MAGO
        mago.SwitchCurrentControlScheme(
            "keyboard+controller",
            teclado,
            gamepadMago
        );

        // Asociar las acciones con los dispositivos de cada jugador
        soldado.user.AssociateActionsWithUser(soldado.actions);
        mago.user.AssociateActionsWithUser(mago.actions);

        Debug.Log("================================");
        Debug.Log(
            "SOLDADO  Keyboard + " +
            gamepadSoldado.displayName
        );

        Debug.Log(
            "MAGO  Keyboard + " +
            gamepadMago.displayName
        );

        Debug.Log("================================");

        // Mostrar qué dispositivos quedaron realmente asociados
        Debug.Log(
            "SOLDADO dispositivos: " +
            string.Join(", ", soldado.devices)
        );

        Debug.Log(
            "MAGO dispositivos: " +
            string.Join(", ", mago.devices)
        );
    }
}