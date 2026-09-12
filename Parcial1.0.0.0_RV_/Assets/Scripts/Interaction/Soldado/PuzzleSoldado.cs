using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class PuzzleSoldado : MonoBehaviour
{
    [Header("Interfaz (opcional, igual que en PuzzleMago)")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private TextMeshProUGUI secuenciaActual;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private GameObject progressPanel;

    [Header("Resultado")]
    [SerializeField] private TextMeshProUGUI mensajeResultado;
    [SerializeField] private Button botonAccion;
    [SerializeField] private TextMeshProUGUI textoBoton;

    [Header("Espadas")]
    [Tooltip("Las espadas en el orden CORRECTO en el que deben ser seleccionadas.")]
    [SerializeField] private EspadaInteractable[] espadasEnOrdenCorrecto;

    [Tooltip("Si está activo, las espadas empiezan desactivadas en la escena y se activan (aparecen) cuando el mago completa su puzzle.")]
    [SerializeField] private bool espadasOcultasAlInicio = true;

    [Header("Eventos")]
    [Tooltip("Se dispara SOLO cuando la secuencia se completa correctamente. Aquí se engancha el PortalManager.")]
    public UnityEvent onPuzzleSoldadoCompletado;

    private string[] solucionIds;
    private string secuenciaJugador = ""; // concatenación de ids seleccionados, separados por '-'
    private int seleccionadas = 0;
    private bool puzzleHabilitado = false;
    private bool puzzleCompletado = false;

    public bool PuzzleHabilitado => puzzleHabilitado;
    public bool PuzzleCompletado => puzzleCompletado;

    private void Awake()
    {
        // Construimos la solución a partir del orden asignado en el inspector
        solucionIds = new string[espadasEnOrdenCorrecto.Length];
        for (int i = 0; i < espadasEnOrdenCorrecto.Length; i++)
        {
            solucionIds[i] = espadasEnOrdenCorrecto[i] != null ? espadasEnOrdenCorrecto[i].EspadaId : "";
        }
    }

    private void Start()
    {
        if (espadasOcultasAlInicio)
        {
            SetEspadasActivas(false);
        }

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (progressPanel != null)
            progressPanel.SetActive(false);
    }

    /// <summary>
    /// Llamado desde el evento onPuzzleMagoCompletado del PuzzleMago.
    /// </summary>
    public void HabilitarPuzzle()
    {
        puzzleHabilitado = true;
        SetEspadasActivas(true);
        Debug.Log("Puzzle del soldado habilitado. Las espadas han aparecido.");
    }

    private void SetEspadasActivas(bool activas)
    {
        if (espadasEnOrdenCorrecto == null)
            return;

        foreach (EspadaInteractable espada in espadasEnOrdenCorrecto)
        {
            if (espada != null)
            {
                espada.gameObject.SetActive(activas);
            }
        }
    }

    public void SeleccionarEspada(string id, EspadaInteractable espada)
    {
        if (!puzzleHabilitado || puzzleCompletado)
            return;

        if (seleccionadas >= solucionIds.Length)
            return;

        if (seleccionadas == 0 && progressPanel != null)
        {
            progressPanel.SetActive(true);
        }

        secuenciaJugador += (secuenciaJugador.Length > 0 ? "-" : "") + id;
        seleccionadas++;

        ActualizarUI();

        if (seleccionadas == solucionIds.Length)
        {
            ComprobarSolucion();
        }
    }

    private void ActualizarUI()
    {
        if (progressText != null)
        {
            progressText.text = "ESPADAS " + seleccionadas + "/" + solucionIds.Length;
        }

        if (secuenciaActual != null)
        {
            secuenciaActual.text = secuenciaJugador;
        }
    }

    private void ComprobarSolucion()
    {
        if (progressPanel != null)
            progressPanel.SetActive(false);

        if (puzzlePanel != null)
            puzzlePanel.SetActive(true);

        string solucionCompleta = string.Join("-", solucionIds);

        if (secuenciaJugador == solucionCompleta)
        {
            puzzleCompletado = true;

            Debug.Log("¡PUZZLE DEL SOLDADO COMPLETADO!");

            if (mensajeResultado != null)
                mensajeResultado.text = "LAS ESPADAS ESTÁN EN ORDEN";

            if (textoBoton != null)
                textoBoton.text = "CONTINUAR";

            if (botonAccion != null)
                botonAccion.gameObject.SetActive(true);

            onPuzzleSoldadoCompletado?.Invoke();
        }
        else
        {
            Debug.Log("Orden de espadas incorrecto.");

            if (mensajeResultado != null)
                mensajeResultado.text = "LAS ESPADAS NO ESTÁN EN EL ORDEN CORRECTO";

            if (textoBoton != null)
                textoBoton.text = "REINICIAR";

            if (botonAccion != null)
                botonAccion.gameObject.SetActive(true);
        }
    }

    public void BotonAccion()
    {
        if (puzzleCompletado)
        {
            if (puzzlePanel != null)
                puzzlePanel.SetActive(false);
        }
        else
        {
            ReiniciarPuzzle();
        }
    }

    public void ReiniciarPuzzle()
    {
        secuenciaJugador = "";
        seleccionadas = 0;
        puzzleCompletado = false;

        if (espadasEnOrdenCorrecto != null)
        {
            foreach (EspadaInteractable espada in espadasEnOrdenCorrecto)
            {
                if (espada != null)
                    espada.ReiniciarEspada();
            }
        }

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (progressPanel != null)
            progressPanel.SetActive(false);

        ActualizarUI();

        Debug.Log("Puzzle del soldado reiniciado.");
    }
}