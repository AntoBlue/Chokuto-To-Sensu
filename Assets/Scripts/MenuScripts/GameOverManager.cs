using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverCanvas;

    private bool gameIsOver = false;

    /// <summary>
    /// Chiamare questa funzione per attivare il Game Over.
    /// Ferma il tempo, mostra la canvas e stampa il tempo passato.
    /// </summary>
    public void TriggerGameOver()
    {
        if (gameIsOver) return;

        gameIsOver = true;
        Time.timeScale = 0f;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);
        
    }

    /// <summary>
    /// Bottone per tornare al menù principale (index 0).
    /// </summary>
    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // Sicurezza: riattiva sempre il tempo
        SceneManager.LoadScene(0);
    }
}