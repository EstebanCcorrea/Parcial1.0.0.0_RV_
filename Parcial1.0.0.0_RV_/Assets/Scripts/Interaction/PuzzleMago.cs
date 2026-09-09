using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PuzzleMago : MonoBehaviour
{
    [Header("Interfaz")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private TextMeshProUGUI palabraActual;
    [SerializeField] private TextMeshProUGUI runeProgressText;
    [SerializeField] private GameObject progressPanel;

    [Header("Resultado")]
    [SerializeField] private TextMeshProUGUI mensajeResultado;
    [SerializeField] private Button botonAccion;
    [SerializeField] private TextMeshProUGUI textoBoton;

    [Header("Solución")]
    [SerializeField] private string solucion = "PORTA";
    [Header("Runas")]
    [SerializeField] private RunaInteractable[] runas;

    private string palabraJugador = "";
    private bool puzzleCompletado = false;

    public void IniciarPuzzle()
    {
        palabraJugador = "";
        puzzleCompletado = false;

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (progressPanel != null)
            progressPanel.SetActive(false);

        ActualizarUI();
    }

    public void SeleccionarRuna(string letra)
    {
        if (puzzleCompletado)
            return;

        if (palabraJugador.Length >= solucion.Length)
            return;

        // Mostrar el indicador al recoger la primera runa
        if (palabraJugador.Length == 0 && progressPanel != null)
        {
            progressPanel.SetActive(true);
        }

        palabraJugador += letra.ToUpper();

        ActualizarUI();

        Debug.Log("Palabra actual: " + palabraJugador);

        // Cuando se recogen las 5 runas
        if (palabraJugador.Length == solucion.Length)
        {
            ComprobarSolucion();
        }
    }

    private void ActualizarUI()
    {
        string palabraVisual = CrearPalabraVisual();

        if (palabraActual != null)
        {
            palabraActual.text = palabraVisual;
        }

        if (runeProgressText != null)
        {
            runeProgressText.text =
                "RUNAS " +
                palabraJugador.Length +
                "/" +
                solucion.Length +
                "\n" +
                palabraVisual;
        }
    }

    private string CrearPalabraVisual()
    {
        string resultado = "";

        for (int i = 0; i < solucion.Length; i++)
        {
            if (i < palabraJugador.Length)
                resultado += palabraJugador[i] + " ";
            else
                resultado += "_ ";
        }

        return resultado;
    }

    private void ComprobarSolucion()
    {
        // Ocultar el pequeño indicador
        if (progressPanel != null)
            progressPanel.SetActive(false);

        // Mostrar el panel grande
        if (puzzlePanel != null)
            puzzlePanel.SetActive(true);

        if (palabraJugador == solucion)
        {
            puzzleCompletado = true;

            Debug.Log("¡PUZZLE COMPLETADO!");
            Debug.Log(" El lenguaje antiguo ha sido despertado.");

            if (palabraActual != null)
                palabraActual.text = palabraJugador;

            if (mensajeResultado != null)
                mensajeResultado.text = "EL LENGUAJE HA DESPERTADO";

            if (textoBoton != null)
                textoBoton.text = "CONTINUAR";

            if (botonAccion != null)
                botonAccion.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log(" Orden incorrecto.");

            if (palabraActual != null)
                palabraActual.text = palabraJugador;

            if (mensajeResultado != null)
                mensajeResultado.text =
                    "LAS RUNAS NO ESTÁN EN EL ORDEN CORRECTO";

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
            ContinuarPuzzle();
        }
        else
        {
            ReiniciarPuzzle();
        }
    }

    private void ContinuarPuzzle()
    {
        Debug.Log(" El Mago ha despertado el antiguo lenguaje.");

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        // Más adelante aquí:
        // - Activaremos el mecanismo
        // - Reproduciremos un efecto
        // - Abriremos el camino del Soldado
        // - Etc.
    }

    public void ReiniciarPuzzle()
    {
        palabraJugador = "";
        puzzleCompletado = false;

        // Reiniciar todas las runas
        if (runas != null)
        {
            foreach (RunaInteractable runa in runas)
            {
                if (runa != null)
                {
                    runa.ReiniciarRuna();
                }
            }
        }

      
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

     
        if (progressPanel != null)
            progressPanel.SetActive(false);

        ActualizarUI();

        Debug.Log("Puzzle reiniciado. Las runas están disponibles nuevamente.");
    }
}