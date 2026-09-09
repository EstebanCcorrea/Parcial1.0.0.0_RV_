using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Inmunidad Temporal")]
    [SerializeField] private float invincibilityDuration = 1f;
    private bool isInvincible = false;

    [Header("Eventos (UI, Animaciones, Sonidos)")]
    public UnityEvent<float, float> onHealthChanged; // Pasa (vidaActual, vidaMaxima)
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Notifica la vida inicial a la UI
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>
    /// Método público para aplicar daño desde trampas o scripts de enemigos.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth, maxHealth);
        onTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (invincibilityDuration > 0)
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    /// <summary>
    /// Método público para curar al jugador (ej. habilidades del Mago).
    /// </summary>
    public void Heal(float amount)
    {
        if (currentHealth <= 0) return; // No cura si ya murió

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>
    /// Restablece la salud al máximo y permite revivir al personaje.
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        onDeath?.Invoke();
        Debug.Log($"{gameObject.name} ha muerto.");
        
        // Opcional: Desactivar el PlayerController autónomamente sin modificar su script
        var controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    // Opcional: Detectar daño por zonas de peligro o trampas con Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard")) // O el tag que usen para trampas/ataques
        {
            TakeDamage(10f); // Valor de daño por defecto de la trampa
        }
    }
}