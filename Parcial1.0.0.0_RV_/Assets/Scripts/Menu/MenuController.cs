using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Paneles Principales")]
    [SerializeField] private GameObject menuPrincipalPanel;
    [SerializeField] private GameObject menuPausaPanel;

    [Header("Subpaneles")]
    [SerializeField] private GameObject menuConfigPanel;
    [SerializeField] private GameObject menuInstrucPanel;

    private void Start()
    {
        AbrirMenuPrincipal();
    }

    // --- MÉTODOS DEL MENÚ PRINCIPAL ---
    public void AbrirMenuPrincipal()
    {
        DesactivarTodosLosPaneles();
        if (menuPrincipalPanel != null) menuPrincipalPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    public void CerrarMenuPrincipal()
    {
        if (menuPrincipalPanel != null) menuPrincipalPanel.SetActive(false);
    }

    // --- MÉTODOS DE SUBMENÚS ---
    public void AbrirConfiguracion()
    {
        DesactivarTodosLosPaneles();
        if (menuConfigPanel != null) menuConfigPanel.SetActive(true);
    }

    public void AbrirInstrucciones()
    {
        DesactivarTodosLosPaneles();
        if (menuInstrucPanel != null) menuInstrucPanel.SetActive(true);
    }

    public void VolverAlMenuPrincipal()
    {
        AbrirMenuPrincipal();
    }

    // --- MÉTODOS DEL MENÚ DE PAUSA ---
    public void AbrirPausa()
    {
        DesactivarTodosLosPaneles();
        if (menuPausaPanel != null) menuPausaPanel.SetActive(true);
        Time.timeScale = 0f; // Congela el tiempo
    }

    // Tanto 'ButtonContinuar' como la 'X' / 'SalirMenu (1)' pueden usar esto
    public void ReanudarJuego()
    {
        DesactivarTodosLosPaneles();
        Time.timeScale = 1f; // Devuelve el tiempo a la normalidad
    }

    public void CerrarPausa()
    {
        ReanudarJuego(); // Oculta la pausa y reanuda el tiempo
    }

    // --- MÉTODO SALIR ---
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    private void DesactivarTodosLosPaneles()
    {
        if (menuPrincipalPanel != null) menuPrincipalPanel.SetActive(false);
        if (menuPausaPanel != null) menuPausaPanel.SetActive(false);
        if (menuConfigPanel != null) menuConfigPanel.SetActive(false);
        if (menuInstrucPanel != null) menuInstrucPanel.SetActive(false);
    }
}