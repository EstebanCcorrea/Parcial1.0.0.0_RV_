using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Mensajes")]
    [TextArea(2, 5)]
    [SerializeField] private string[] messages;

    private int currentMessage = 0;
    private bool dialogueActive = false;

    private float lastAdvanceTime = -1f;

    [SerializeField] private float advanceCooldown = 0.25f;

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void StartDialogue()
    {
        if (messages == null || messages.Length == 0)
            return;

        currentMessage = 0;
        dialogueActive = true;

        panel.SetActive(true);
        ShowCurrentMessage();
    }

    public void NextMessage()
    {
        if (!dialogueActive)
            return;

        // Evita que una sola pulsación avance varios mensajes
        if (Time.unscaledTime - lastAdvanceTime < advanceCooldown)
            return;

        lastAdvanceTime = Time.unscaledTime;

        currentMessage++;

        if (currentMessage >= messages.Length)
        {
            CloseDialogue();
            return;
        }

        ShowCurrentMessage();
    }
    private void ShowCurrentMessage()
    {
        dialogueText.text = messages[currentMessage];
    }

    private void CloseDialogue()
    {
        dialogueActive = false;
        panel.SetActive(false);
    }
}