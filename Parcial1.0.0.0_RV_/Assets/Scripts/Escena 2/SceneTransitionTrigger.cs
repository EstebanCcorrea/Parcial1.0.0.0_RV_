using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Configuración de Transición")]
    [Tooltip("Nombre exacto de la siguiente escena en Build Settings (ej. Scene_03_Artefacto)")]
    [SerializeField] private string nextSceneName = "Scene_03_Artefacto";

    [Header("Filtros")]
    [SerializeField] private string mageName = "Mago";
    [SerializeField] private string soldierName = "Soldado";

    private bool isMageInZone = false;
    private bool isSoldierInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == mageName) isMageInZone = true;
        if (other.gameObject.name == soldierName) isSoldierInZone = true;

        CheckTransition();
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == mageName) isMageInZone = false;
        if (other.gameObject.name == soldierName) isSoldierInZone = false;
    }

    private void CheckTransition()
    {
        // Si ambos jugadores están en el punto de salida, cambia de escena
        if (isMageInZone && isSoldierInZone)
        {
            Debug.Log($"Cargando siguiente escena: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}