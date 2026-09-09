using UnityEngine;

public class CrystalSocket : MonoBehaviour
{
    [Header("Referencias del Puzzle")]
    [Tooltip("Transform vacío que marca la posición y rotación exacta donde encajará el cristal")]
    [SerializeField] private Transform snapPoint;
    [Tooltip("GameObject de la puerta que se desactivará/abrirá al completar el puzzle")]
    [SerializeField] private GameObject doorToOpen;//si

    [Header("Visualización con Gizmos")]
    [SerializeField] private Color gizmoColor = Color.cyan;
    [SerializeField] private float gizmoRadius = 0.5f;

    private bool isEnabled = false; // Se activa únicamente cuando el Soldado acciona la polea
    private bool isPlaced = false;

    /// <summary>
    /// Llamado desde el script PulleyMechanism cuando el Soldado activa la polea.
    /// </summary>
    public void EnableSocket()
    {
        isEnabled = true;
        Debug.Log("Socket habilitado. El Mago ya puede colocar el cristal.");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el socket aún no está activo por la polea o ya tiene el cristal, ignorar
        if (!isEnabled || isPlaced) return;

        // Verificar si el objeto que entra es el Cristal
        PickableCrystal crystal = other.GetComponent<PickableCrystal>();
        if (crystal != null)
        {
            PlaceCrystal(crystal);
        }
    }

    private void PlaceCrystal(PickableCrystal crystal)
    {
        isPlaced = true;

        // 1. Soltar el cristal de las manos del Mago si lo llevaba cargado
        crystal.Drop();

        // 2. Alinear y emparentar el cristal al Snap Point del zócalo
        Transform targetTransform = snapPoint != null ? snapPoint : transform;
        crystal.transform.SetParent(targetTransform);
        crystal.transform.localPosition = Vector3.zero;
        crystal.transform.localRotation = Quaternion.identity;

        // 3. Desactivar físicas y el script del cristal para congelarlo en su sitio
        var rb = crystal.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        
        var col = crystal.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        crystal.enabled = false;

        // 4. Abrir la puerta de salida
        OpenDoor();
    }

    private void OpenDoor()
    {
        if (doorToOpen != null)
        {
            doorToOpen.SetActive(false); // O activa el Animator de la puerta si tiene uno
            Debug.Log("¡Puzzle completado! Puerta abierta hacia la Escena 3.");
        }
    }

    // Dibuja una esfera guía en la vista de escena (Scene View) de Unity
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Vector3 center = snapPoint != null ? snapPoint.position : transform.position;
        Gizmos.DrawWireSphere(center, gizmoRadius);
    }
}