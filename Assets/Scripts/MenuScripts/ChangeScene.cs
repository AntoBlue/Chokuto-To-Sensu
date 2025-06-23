using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // --- Campi Serializzati ---
    // Questi campi appariranno nell'Inspector di Unity e potranno essere assegnati lì.

    [SerializeField] [Tooltip("Il nome della scena del menu principale.")]
    //private SceneAsset MenuScene;
    private string mainmenuSceneName = "MainMenuScene"; // Nome di default per la scena del menu principale

    [SerializeField]
    [Tooltip("Il nome della scena di gioco principale.")]
    //private SceneAsset GameScene;
    private string gameSceneName = "GameScene";   // Nome di default per la scena di gioco

    [SerializeField]
    //private SceneAsset CombatScene;
    private string combatSceneName = "CombatScene"; 
    
    [SerializeField]
    //private SceneAsset StatuePowerScene;
    private string statuePowerSceneName = "StatuePowerScene"; 
    
    [SerializeField]
    //private SceneAsset CameraFollowScene;
    private string CameraFollowSceneName = "CameraFollowScene";
    
    [SerializeField]
    //private SceneAsset AIScene;
    private string AISceneName = "AIScene"; 
    
    
    
    
    
    
    [SerializeField]
    [Tooltip("La Canvas che rappresenta il menu di pausa. Verrà disabilitata/abilitata.")]
    private GameObject pauseCanvas; // Riferimento all'oggetto GameObject della Canvas di Pausa


    void Start()
    {
        gameSceneName = gameSceneName;
        mainmenuSceneName = mainmenuSceneName;
        combatSceneName = combatSceneName;
        statuePowerSceneName = statuePowerSceneName;
        CameraFollowSceneName = CameraFollowSceneName;
        AISceneName = AISceneName;
    }
    // --- Funzioni Pubbliche ---
    // Queste funzioni possono essere richiamate da altri script o da eventi UI (es. pulsanti).
    
    
    /// <summary>
    /// Carica la scena specificata in 'mainmenuSceneName'.
    /// </summary>
    public void GoToMainMenu()
    {
        // Controlla se il nome della scena è valido (non vuoto)
        if (!string.IsNullOrEmpty(mainmenuSceneName))
        {
            Debug.Log($"Caricamento scena: {mainmenuSceneName}");
            SceneManager.LoadScene(mainmenuSceneName);
        }
        else
        {
            Debug.LogError("Il nome della scena 'MainMenu' non è stato impostato! Si prega di configurarlo nell'Inspector.");
        }
    }

    /// <summary>
    /// Carica la scena specificata in 'gameSceneName'.
    /// </summary>
    public void GoToGameScene()
    {
        // Controlla se il nome della scena è valido (non vuoto)
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            Debug.Log($"Caricamento scena: {gameSceneName}");
            Time.timeScale = 1f;
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Il nome della scena 'GameScene' non è stato impostato! Si prega di configurarlo nell'Inspector.");
        }
    }
    
    public void GoToCombatScene()
    {
        // Controlla se il nome della scena è valido (non vuoto)
        if (!string.IsNullOrEmpty(combatSceneName))
        {
            Debug.Log($"Caricamento scena: {combatSceneName}");
            SceneManager.LoadScene(combatSceneName);
        }
        else
        {
            Debug.LogError("Il nome della scena 'combatScene' non è stato impostato! Si prega di configurarlo nell'Inspector.");
        }
    }
    
    public void GoToCameraFollowScene()
    {
        // Controlla se il nome della scena è valido (non vuoto)
        if (!string.IsNullOrEmpty(CameraFollowSceneName))
        {
            Debug.Log($"Caricamento scena: {CameraFollowSceneName}");
            SceneManager.LoadScene(CameraFollowSceneName);
        }
        else
        {
            Debug.LogError("Il nome della scena 'GoToCameraFollowScene' non è stato impostato! Si prega di configurarlo nell'Inspector.");
        }
    }
    
    public void GoToStatueScene()
    {
        // Controlla se il nome della scena è valido (non vuoto)
        if (!string.IsNullOrEmpty(statuePowerSceneName))
        {
            Debug.Log($"Caricamento scena: {statuePowerSceneName}");
            SceneManager.LoadScene(statuePowerSceneName);
        }
        else
        {
            Debug.LogError("Il nome della scena 'statuePowerScene' non è stato impostato! Si prega di configurarlo nell'Inspector.");
        }
    }
    
    public void GoToAIScene()
    {
        // Controlla se il nome della scena è valido (non vuoto)
        if (!string.IsNullOrEmpty(AISceneName))
        {
            Debug.Log($"Caricamento scena: {AISceneName}");
            SceneManager.LoadScene(AISceneName);
        }
        else
        {
            Debug.LogError("Il nome della scena 'AIScene' non è stato impostato! Si prega di configurarlo nell'Inspector.");
        }
    }
    

    /// <summary>
    /// Disabilita la Canvas di pausa.
    /// Questo significa che il menu di pausa non sarà più visibile.
    /// </summary>
    public void StopPause()
    {
        // Controlla se il riferimento alla Canvas di pausa è stato assegnato
        if (pauseCanvas != null)
        {
            Debug.Log("Disabilitazione della PauseCanvas.");
            pauseCanvas.SetActive(false); // Disabilita l'oggetto GameObject della Canvas
            Time.timeScale = 1f; // Ripristina il tempo di gioco (se era stato messo in pausa)
        }
        else
        {
            Debug.LogError("La 'PauseCanvas' non è stata assegnata! Si prega di trascinare la Canvas nell'Inspector.");
        }
    }

    /// <summary>
    /// Abilita la Canvas di pausa.
    /// Questo significa che il menu di pausa diventerà visibile.
    /// Tipicamente, questa funzione andrebbe abbinata a una logica che mette in pausa il gioco (es. Time.timeScale = 0).
    /// </summary>
    public void StartPause()
    {
        if (pauseCanvas != null)
        {
            Debug.Log("Abilitazione della PauseCanvas.");
            pauseCanvas.SetActive(true); // Abilita l'oggetto GameObject della Canvas
            Time.timeScale = 0f; // Ferma il tempo di gioco
        }
        else
        {
            Debug.LogError("La 'PauseCanvas' non è stata assegnata! Si prega di trascinare la Canvas nell'Inspector.");
        }
    }

    /// <summary>
    /// Funzione per uscire dall'applicazione (solo per build del gioco).
    /// Questa funzione non ha effetto nell'editor di Unity.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco.");
#if UNITY_EDITOR
        // Se siamo nell'editor, non si può uscire dall'applicazione
        // Si può usare UnityEditor.EditorApplication.isPlaying = false; per fermare il Play Mode,
        // ma ciò richiede l'importazione di UnityEditor, che non è disponibile nelle build finali.
        // Per semplicità e compatibilità con le build, usiamo Debug.Log qui.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Esce dall'applicazione solo nella build finale
#endif
    }
}
