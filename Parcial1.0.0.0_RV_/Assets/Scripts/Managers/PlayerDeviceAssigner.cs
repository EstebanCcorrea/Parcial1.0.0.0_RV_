using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Asigna manualmente los dispositivos de input a cada PlayerInput al iniciar
/// el juego, en vez de depender del auto-switch / join-by-button-press de
/// PlayerInputManager (que puede cruzar los controles entre partidas).
///
/// Reglas:
/// - Si hay al menos 1 gamepad conectado -> Soldado usa el primer gamepad.
/// - Si hay un SEGUNDO gamepad conectado -> Mago usa ese segundo gamepad.
/// - Si NO hay un segundo gamepad -> Mago usa Teclado + Mouse.
/// - Si no hay NINGÚN gamepad -> Soldado usa Teclado + Mouse (fallback).
///
/// 
/// </summary>
public class PlayerDeviceAssigner : MonoBehaviour
{
    [SerializeField] private PlayerInput soldadoInput;
    [SerializeField] private PlayerInput magoInput;

    private const string SCHEME_GAMEPAD = "Control";
    private const string SCHEME_KEYBOARD = "Teclado&Mouse";

    private void Start()
    {
        AssignDevices();
    }

    private void AssignDevices()
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count >= 1)
        {
            AssignGamepad(soldadoInput, gamepads[0]);

            if (gamepads.Count >= 2)
            {
                AssignGamepad(magoInput, gamepads[1]);
            }
            else
            {
                AssignKeyboardMouse(magoInput);
            }
        }
        else
        {
            // No hay ningún gamepad conectado: ambos no pueden usar el mismo
            // teclado a la vez de forma confiable, así que al menos dejamos
            // a Soldado con teclado como fallback y avisamos en consola.
            AssignKeyboardMouse(soldadoInput);
            Debug.LogWarning("No se detectó ningún gamepad. Mago se quedará sin dispositivo asignado hasta que conectes un segundo control.");
        }
    }

    private void AssignGamepad(PlayerInput playerInput, Gamepad gamepad)
    {
        if (playerInput == null || gamepad == null) return;
        playerInput.SwitchCurrentControlScheme(SCHEME_GAMEPAD, gamepad);
        Debug.Log($"[PlayerDeviceAssigner] {playerInput.gameObject.name} -> Gamepad ({gamepad.displayName})");
    }

    private void AssignKeyboardMouse(PlayerInput playerInput)
    {
        if (playerInput == null) return;
        playerInput.SwitchCurrentControlScheme(SCHEME_KEYBOARD, Keyboard.current, Mouse.current);
        Debug.Log($"[PlayerDeviceAssigner] {playerInput.gameObject.name} -> Teclado + Mouse");
    }
}
