using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PulleyMechanism : MonoBehaviour
{
    [Header("Filtro de Jugador")]
    [Tooltip("Escribe el nombre exacto del GameObject del Soldado en la jerarquía")]
    [SerializeField] private string soldierGameObjectName = "Soldado";

    [Header("Interfaz de Usuario (UI)")]
    [Tooltip("Panel UI que notificará la apertura del área del cristal")]
    [SerializeField] private GameObject feedbackPanel;
    [Tooltip("Tiempo en segundos que se mostrará el panel")]
    [SerializeField] private float panelDuration = 3.5f;

    [Header("Componentes y Animación")]
    [Tooltip("Componente Animator de la polea (opcional)")]
    [SerializeField] private Animator pulleyAnimator;
    [Tooltip("Nombre del parámetro Trigger en el Animator para activar la polea")]
    [SerializeField] private string animationTriggerName = "Activate";

    [Header("Conexión con el Socket")]
    [Tooltip("Arrastra aquí el GameObject que tendrá el script CrystalSocket")]
    [SerializeField] private CrystalSocket crystalSocket;

    private bool isActivated = false;
    private bool isSoldierInRange = false;

    private void Start()
    {
        // Nos aseguramos de que el panel inicie oculto
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == soldierGameObjectName)
        {
            isSoldierInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == soldierGameObjectName)
        {
            isSoldierInRange = false;
        }
    }

    /// <summary>
    /// Este método se enlaza en el PlayerInput del Soldado mediante la acción Interact.
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed || isActivated || !isSoldierInRange) return;

        ActivatePulley();
    }

    private void ActivatePulley()
    {
        isActivated = true;

        // 1. Ejecutar animación de la polea si existe
        if (pulleyAnimator != null)
        {
            pulleyAnimator.SetTrigger(animationTriggerName);
        }

        // 2. Habilitar la receptáculo/socket para recibir el cristal
        if (crystalSocket != null)
        {
            crystalSocket.EnableSocket();
        }

        // 3. Mostrar el panel de alerta temporal
        if (feedbackPanel != null)
        {
            StartCoroutine(ShowFeedbackRoutine());
        }
    }

    private IEnumerator ShowFeedbackRoutine()
    {
        feedbackPanel.SetActive(true);
        yield return new WaitForSeconds(panelDuration);
        feedbackPanel.SetActive(false);
    }
}