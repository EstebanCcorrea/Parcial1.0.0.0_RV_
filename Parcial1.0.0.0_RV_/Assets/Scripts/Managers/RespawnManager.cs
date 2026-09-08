using System.Collections;
using UnityEngine;
using TMPro;

public class RespawnManager : MonoBehaviour
{
    [Header("UI Game Over - Soldado (P1)")]
    [SerializeField] private GameObject gameOverPanelP1;
    [SerializeField] private TextMeshProUGUI countdownTextP1;

    [Header("UI Game Over - Mago (P2)")]
    [SerializeField] private GameObject gameOverPanelP2;
    [SerializeField] private TextMeshProUGUI countdownTextP2;

    [Header("Puntos de Aparición (Spawn Points)")]
    [SerializeField] private Transform player1SpawnPoint;
    [SerializeField] private Transform player2SpawnPoint;

    [Header("Referencias a los Jugadores")]
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    [Header("Configuración")]
    [SerializeField] private float respawnDelay = 5f;

    private void Start()
    {
        if (gameOverPanelP1 != null) gameOverPanelP1.SetActive(false);
        if (gameOverPanelP2 != null) gameOverPanelP2.SetActive(false);
    }

    public void OnPlayer1Died()
    {
        StartCoroutine(RespawnRoutine(player1Health, player1SpawnPoint, gameOverPanelP1, countdownTextP1));
    }

    public void OnPlayer2Died()
    {
        StartCoroutine(RespawnRoutine(player2Health, player2SpawnPoint, gameOverPanelP2, countdownTextP2));
    }

    private IEnumerator RespawnRoutine(PlayerHealth playerHealth, Transform spawnPoint, GameObject gameOverPanel, TextMeshProUGUI countdownText)
    {
        // ================= ETAPA 1: MUERTE Y ESPERA (5s) =================

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        playerHealth.gameObject.SetActive(false);

        float timer = respawnDelay;
        while (timer > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = $"Reapareciendo en {Mathf.CeilToInt(timer)}s...";
            }
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        // ================= ETAPA 2: REAPARICIÓN Y REINICIO =================

        if (spawnPoint != null)
        {
            Rigidbody rb = playerHealth.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = Vector3.zero;

            playerHealth.transform.position = spawnPoint.position;
            playerHealth.transform.rotation = spawnPoint.rotation;
        }

        // 1. Activar al jugador
        playerHealth.gameObject.SetActive(true);

        var controller = playerHealth.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = true;

        // 2. Usar ResetHealth para forzar la salud al 100% sin importar si estaba en 0
        playerHealth.ResetHealth();

        // 3. Ocultar el panel de Game Over
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }
}