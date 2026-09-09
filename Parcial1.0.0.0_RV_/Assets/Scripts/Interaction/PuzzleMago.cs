using UnityEngine;
using TMPro;

public class PuzzleMago : MonoBehaviour
{
    [Header("Interfaz")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private TextMeshProUGUI palabraActual;
    [SerializeField] private TextMeshProUGUI runeProgressText;
    [SerializeField] private GameObject progressPanel;

    [Header("Solución")]
    [SerializeField] private string solucion = "PORTA";

    private string palabraJugador = "";

    public void IniciarPuzzle()
    {
        palabraJugador = "";

        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(true);
        }

        ActualizarUI();
    }

    public void SeleccionarRuna(string letra)
    {
        if (palabraJugador.Length >= solucion.Length)
            return;

        // Mostrar el indicador cuando se recoge la primera runa
        if (palabraJugador.Length == 0 && progressPanel != null)
        {
            progressPanel.SetActive(true);
        }

        palabraJugador += letra;

        ActualizarUI();

        Debug.Log("Palabra actual: " + palabraJugador);

        if (palabraJugador.Length == solucion.Length)
        {
            ComprobarSolucion();
        }
    }
    private void ActualizarUI()
    {
        // Actualiza la palabra del puzzle grande
        if (palabraActual != null)
        {
            palabraActual.text = CrearPalabraVisual();
        }

        // Actualiza el indicador pequeño de progreso
        if (runeProgressText != null)
        {
            runeProgressText.text =
                "RUNAS " +
                palabraJugador.Length +
                "/" +
                solucion.Length +
                "\n" +
                CrearPalabraVisual();
        }
    }

    private string CrearPalabraVisual()
    {
        string resultado = "";

        for (int i = 0; i < solucion.Length; i++)
        {
            if (i < palabraJugador.Length)
            {
                resultado += palabraJugador[i] + " ";
            }
            else
            {
                resultado += "_ ";
            }
        }

        return resultado;
    }

    private void ComprobarSolucion()
    {
        if (palabraJugador == solucion)
        {
            Debug.Log("¡PUZZLE COMPLETADO!");

            // Más adelante aquí podemos:
            // - Mostrar el panel final
            // - Reproducir un sonido
            // - Activar un efecto mágico
            // - Revelar la información para el Soldado
            // - Activar el mecanismo final
        }
        else
        {
            Debug.Log("Orden incorrecto.");
        }
    }

    public void ReiniciarPuzzle()
    {
        palabraJugador = "";

        ActualizarUI();

        Debug.Log("Puzzle reiniciado.");
    }
}