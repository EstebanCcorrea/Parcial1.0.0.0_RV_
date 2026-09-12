using UnityEngine;

public class EspadaInteractable : Interactable
{
    [Header("Configuración de la espada")]
    [Tooltip("Identificador único de esta espada, ej: '1', '2', '3'... o un nombre corto.")]
    [SerializeField] private string espadaId;

    [SerializeField] private PuzzleSoldado puzzleSoldado;

    [Header("Efecto visual")]
    [SerializeField] private GameObject efectoIluminado;
    [SerializeField] private Renderer espadaRenderer;
    [SerializeField] private Color colorIluminado = Color.yellow;

    private Color colorOriginal;
    private bool selected = false;

    public string EspadaId => espadaId;

    private void Awake()
    {
        if (espadaRenderer != null)
        {
            colorOriginal = espadaRenderer.material.color;
        }
    }

    public override void Interact()
    {
        if (selected)
            return;

        // No se puede tocar la espada hasta que el puzzle esté habilitado
        if (puzzleSoldado == null || !puzzleSoldado.PuzzleHabilitado)
            return;

        selected = true;

        Iluminar();

        if (puzzleSoldado != null)
        {
            puzzleSoldado.SeleccionarEspada(espadaId, this);
        }

        Debug.Log("Espada seleccionada: " + espadaId);
    }

    private void Iluminar()
    {
        if (efectoIluminado != null)
        {
            efectoIluminado.SetActive(true);
        }

        if (espadaRenderer != null)
        {
            espadaRenderer.material.color = colorIluminado;
            espadaRenderer.material.EnableKeyword("_EMISSION");
            espadaRenderer.material.SetColor("_EmissionColor", colorIluminado);
        }
    }

    private void Apagar()
    {
        if (efectoIluminado != null)
        {
            efectoIluminado.SetActive(false);
        }

        if (espadaRenderer != null)
        {
            espadaRenderer.material.color = colorOriginal;
            espadaRenderer.material.DisableKeyword("_EMISSION");
        }
    }

    public void ReiniciarEspada()
    {
        selected = false;
        Apagar();
    }
}
