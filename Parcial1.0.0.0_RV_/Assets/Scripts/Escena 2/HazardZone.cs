using UnityEngine;

public class HazardZone : MonoBehaviour
{
    [Header("Configuración Elemental")]
    [Tooltip("El jugador asignado aquí PODRÁ cruzar de forma segura")]
    [SerializeField] private string safePlayerName = "Mago"; // Para el charco azul pones Mago, para el rojo Soldado

    [Header("Daño")]//si
    [SerializeField] private float damageAmount = 10f;
    [Tooltip("Tiempo en segundos entre cada aplicación de daño si se queda parado sobre el charco")]
    [SerializeField] private float damageInterval = 1f;

    private float nextDamageTime;

    private void OnTriggerStay(Collider other)
    {
        // 1. Si es el jugador seguro para este charco, no hace nada
        if (other.gameObject.name == safePlayerName) return;

        // 2. Si entra el jugador equivocado, le aplicamos daño periódicamente
        if (Time.time >= nextDamageTime)
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageInterval;
                Debug.Log($"¡{other.gameObject.name} pisó un charco peligroso y recibió {damageAmount} de daño!");
            }
        }
    }
}