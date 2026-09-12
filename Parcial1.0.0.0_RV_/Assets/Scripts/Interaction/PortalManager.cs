using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [Header("Portales")]
    [Tooltip("Portal que aparece cerca del Mago.")]
    [SerializeField] private GameObject portalMago;

    [Tooltip("Portal que aparece cerca del Soldado.")]
    [SerializeField] private GameObject portalSoldado;

    [Header("Referencias a los puzzles (opcional, solo para chequeo por consulta)")]
    [SerializeField] private PuzzleMago puzzleMago;
    [SerializeField] private PuzzleSoldado puzzleSoldado;

    private bool magoListo = false;
    private bool soldadoListo = false;

    private void Start()
    {
        if (portalMago != null)
            portalMago.SetActive(false);

        if (portalSoldado != null)
            portalSoldado.SetActive(false);
    }

    /// <summary>
    /// Enganchar en el UnityEvent onPuzzleMagoCompletado del PuzzleMago.
    /// </summary>
    public void NotificarMagoListo()
    {
        magoListo = true;
        Debug.Log("PortalManager: Mago listo.");
        RevisarAmbosPuzzles();
    }

    /// <summary>
    /// Enganchar en el UnityEvent onPuzzleSoldadoCompletado del PuzzleSoldado.
    /// </summary>
    public void NotificarSoldadoListo()
    {
        soldadoListo = true;
        Debug.Log("PortalManager: Soldado listo.");
        RevisarAmbosPuzzles();
    }

    private void RevisarAmbosPuzzles()
    {
        if (magoListo && soldadoListo)
        {
            AbrirPortales();
        }
    }

    private void AbrirPortales()
    {
        Debug.Log("¡Ambos puzzles completados! Abriendo portales hacia la isla.");

        if (portalMago != null)
            portalMago.SetActive(true);

        if (portalSoldado != null)
            portalSoldado.SetActive(true);

        // Aquí después puedes añadir: sonido, partículas, cámara, etc.
    }
}
