using UnityEngine;

public class RunaInteractable : Interactable
{
    [Header("Configuración de la runa")]
    [SerializeField] private string runeValue;

    private bool selected = false;

    public string RuneValue => runeValue;

    public override void Interact()
    {
        if (selected)
            return;

        selected = true;

        Debug.Log("Runa seleccionada: " + runeValue);
    }
}