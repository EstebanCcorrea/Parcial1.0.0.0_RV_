using UnityEngine;

public class RunaInteractable : Interactable
{
    [Header("Configuración de la runa")]
    [SerializeField] private string runeValue;

    [SerializeField] private PuzzleMago puzzleMago;

    [Header("Efecto visual")]
    [Tooltip("Objeto que se activa cuando la runa es seleccionada (ej: un Light, un glow, una partícula).")]
    [SerializeField] private GameObject efectoIluminado;

    [Tooltip("Opcional: si tu runa usa un material con emisión, se puede activar aquí en vez de (o además de) efectoIluminado.")]
    [SerializeField] private Renderer runaRenderer;
    [SerializeField] private Color colorIluminado = Color.cyan;

    private Color colorOriginal;
    private bool selected = false;

    public string RuneValue => runeValue;

    private void Awake()
    {
        if (runaRenderer != null)
        {
            colorOriginal = runaRenderer.material.color;
        }
    }

    public override void Interact()
    {
        if (selected)
            return;

        selected = true;

        Iluminar();

        if (puzzleMago != null)
        {
            puzzleMago.SeleccionarRuna(runeValue);
        }

        Debug.Log("Runa seleccionada: " + runeValue);
    }

    private void Iluminar()
    {
        if (efectoIluminado != null)
        {
            efectoIluminado.SetActive(true);
        }

        if (runaRenderer != null)
        {
            runaRenderer.material.color = colorIluminado;
            // Si el shader soporta emisión (ej. URP Lit / Standard):
            runaRenderer.material.EnableKeyword("_EMISSION");
            runaRenderer.material.SetColor("_EmissionColor", colorIluminado);
        }
    }

    private void Apagar()
    {
        if (efectoIluminado != null)
        {
            efectoIluminado.SetActive(false);
        }

        if (runaRenderer != null)
        {
            runaRenderer.material.color = colorOriginal;
            runaRenderer.material.DisableKeyword("_EMISSION");
        }
    }

    public void ReiniciarRuna()
    {
        selected = false;
        Apagar();
    }
}