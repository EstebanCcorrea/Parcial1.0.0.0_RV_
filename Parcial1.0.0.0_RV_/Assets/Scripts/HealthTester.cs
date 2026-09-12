using UnityEngine;
using UnityEngine.InputSystem; // Importante para el nuevo Input System

public class HealthTester : MonoBehaviour
{
    [Header("Referencias a los Jugadores")]
    [SerializeField] private PlayerHealth player1Health; // Mago
    [SerializeField] private PlayerHealth player2Health; // Soldadosi

    private void Update()
    {
        // Verifica si hay un teclado conectado
        if (Keyboard.current == null) return;

        // --- PRUEBAS JUGADOR 1 ---
        // Tecla 1 (Alpha1): Daño de 10 al Jugador 1
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            if (player1Health != null) player1Health.TakeDamage(10f);
        }
        // Tecla 2 (Alpha2): Cura de 10 al Jugador 1
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            if (player1Health != null) player1Health.Heal(10f);
        }

        // --- PRUEBAS JUGADOR 2 ---
        // Tecla 3 (Alpha3): Daño de 10 al Jugador 2
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            if (player2Health != null) player2Health.TakeDamage(10f);
        }
        // Tecla 4 (Alpha4): Cura de 10 al Jugador 2
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            if (player2Health != null) player2Health.Heal(10f);
        }
    }
}