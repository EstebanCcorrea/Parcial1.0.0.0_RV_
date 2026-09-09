using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Diálogo")]
    [SerializeField] private DialogueUI dialogueUI;

    public virtual void Interact()
    {
        if (dialogueUI != null)
        {
            dialogueUI.StartDialogue();
        }

        Debug.Log("Interacción realizada con: " + gameObject.name);
    }
}