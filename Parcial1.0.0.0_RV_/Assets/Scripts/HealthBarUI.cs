using UnityEngine;
using UnityEngine.UI;
using TMPro; // Opcional: si usas TextMeshPro para el texto

public class HealthBarUI : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private Image healthFillImage; // Imagen de la barra con Fill Amount
    [SerializeField] private Slider healthSlider;   // Opción alternativa usando un Slider
    [SerializeField] private TextMeshProUGUI healthText; // Opción para mostrar números (ej. "80 / 100")

    /// <summary>
    /// Este método se llamará automáticamente a través del evento 'onHealthChanged' de PlayerHealth.
    /// </summary>
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // 1. Si usas Image Fill
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        }

        // 2. Si usas Slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Ajusta el rango al máximo de vida
            healthSlider.value = currentHealth;
        }

        // 3. Texto
        if (healthText != null)
        {
            healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }
}