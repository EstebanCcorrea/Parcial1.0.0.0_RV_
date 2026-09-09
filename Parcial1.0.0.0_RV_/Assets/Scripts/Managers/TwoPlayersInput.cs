using System.Linq;
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

        // =====================================
        // IDENTIFICAR LOS DOS GAMEPADS
        // =====================================

        Gamepad gamepadSoldado = null;
        Gamepad gamepadMago = null;

        foreach (Gamepad gamepad in Gamepad.all)
        {
            Debug.Log(
                "Gamepad detectado: " +
                gamepad.displayName
            );

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

        // =====================================
        // LIMPIAR ASIGNACIONES ACTUALES
        // =====================================

        soldado.user.UnpairDevices();
        mago.user.UnpairDevices();

        // =====================================
        // SOLDADO
        // DualShock
        // =====================================

        soldado.SwitchCurrentControlScheme(
            "Controller",
            gamepadSoldado
        );

        // =====================================
        // MAGO
        // Keyboard + XInput
        // =====================================

        if (Keyboard.current != null)
        {
            mago.SwitchCurrentControlScheme(
                "keyboard+controller",
                Keyboard.current,
                gamepadMago
            );
        }

        Debug.Log("================================");
        Debug.Log(
            "SOLDADO  " +
            gamepadSoldado.displayName
        );
        Debug.Log(
            "MAGO  Keyboard + " +
            gamepadMago.displayName
        );
        Debug.Log("================================");
    }

}