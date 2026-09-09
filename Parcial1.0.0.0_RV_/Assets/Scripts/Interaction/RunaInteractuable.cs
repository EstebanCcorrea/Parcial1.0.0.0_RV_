using UnityEngine;

public class RunaInteractable : Interactable
{
    [Header("Configuración de la runa")]
    [SerializeField] private string runeValue;

    [SerializeField] private PuzzleMago puzzleMago;

    private bool selected = false;

    public string RuneValue => runeValue;

    public override void Interact()
    {
        if (selected)
            return;

        selected = true;

        if (puzzleMago != null)
        {
            puzzleMago.SeleccionarRuna(runeValue);
        }

        Debug.Log("Runa seleccionada: " + runeValue);
    }
}