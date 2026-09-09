using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float interactionDistance = 3f;

    private Interactable currentInteractable;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void Update()
    {
        FindInteractable();
    }

    private void FindInteractable()
    {
        currentInteractable = null;

        Collider[] nearbyObjects = Physics.OverlapSphere(
            transform.position,
            interactionDistance
        );

        float closestDistance = Mathf.Infinity;

        foreach (Collider col in nearbyObjects)
        {
            Interactable interactable = col.GetComponent<Interactable>();

            if (interactable == null)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                col.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentInteractable = interactable;
            }
        }
    }
}