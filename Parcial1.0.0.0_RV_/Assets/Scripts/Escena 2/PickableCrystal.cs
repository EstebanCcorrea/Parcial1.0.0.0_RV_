using UnityEngine;
using UnityEngine.InputSystem;

public class PickableCrystal : MonoBehaviour
{
    [Header("Filtro de Jugador")]
    [Tooltip("Escribe el nombre exacto del GameObject del Mago en la jerarquía")]
    [SerializeField] private string mageGameObjectName = "Mago"; 

    [Header("Posición de Agarre")]
    [Tooltip("Offset relativo a las manos/pecho del Mago")]
    [SerializeField] private Vector3 holdOffset = new Vector3(0f, 1.2f, 0.8f);

    private Transform currentHolder;
    private Rigidbody rb;
    private Collider col;
    private bool isMageInRange = false;
    private Transform mageTransform;

    public bool IsCarried => currentHolder != null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Validamos si el objeto que entra es el Mago por su nombre de GameObject
        if (other.gameObject.name == mageGameObjectName)
        {
            isMageInRange = true;
            mageTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == mageGameObjectName)
        {
            isMageInRange = false;
            mageTransform = null;
        }
    }

    /// <summary>
    /// Este método se enlaza en el PlayerInput del Mago mediante la acción Interact.
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Solo reacciona al presionar la tecla/botón
        if (!context.performed) return;

        // Si no lo tiene nadie y el Mago está cerca, lo agarra
        if (currentHolder == null && isMageInRange)
        {
            PickUp(mageTransform);
        }
        // Si el Mago ya lo tiene en la mano, lo suelta
        else if (currentHolder != null)
        {
            Drop();
        }
    }

    private void PickUp(Transform holder)
    {
        currentHolder = holder;
        
        // Emparentar al Mago para que el cristal lo siga al caminar
        transform.SetParent(holder);
        transform.localPosition = holdOffset;
        transform.localRotation = Quaternion.identity;

        // Desactivar física para que no interfiera con el movimiento del jugador
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;
    }

    public void Drop()
    {
        // Desparentar
        transform.SetParent(null);

        // Reactivar físicas y colisiones
        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        currentHolder = null;
    }
}