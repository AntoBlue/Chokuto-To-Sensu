// Script principale: gestisce la pausa e la selezione dei bottoni del menu pausa
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject pointer;
    private YPositionMover pointerMover;
    
    [Header("Input Asset")]
    [SerializeField] private InputActionAsset inputAsset;

    private EventSystem eventSystem;
    private int selectedIndex = 0;
    private bool isPaused = false;
    public bool IsPaused => isPaused;
    // This is the array that will contain the TextMeshPro references of the buttons.
    private FontSizeController[] buttonTextControllers;

    
    /// <summary>
    /// Player input
    /// </summary>
    [SerializeField] private string gameplayMapName = "Player";
    [SerializeField] private string uiMapName = "UI";
    private InputActionMap gameplayMap;
    private InputActionMap uiMap;
    private InputAction navigateAction;
    private InputAction submitAction;
    
    private float navigationCooldown = 0.2f;
    private float lastNavigationTime = 0f;
    
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player";
    [SerializeField] private string pauseActionName = "Pause";
    private InputAction pauseActionGameplay;
    private InputAction pauseActionUI;
    private bool pauseHandledThisFrame = false;
    

    private void OnEnable()
    {
        
        
        var playerMap = inputActions.FindActionMap(actionMapName, true);
        var uiMap = inputActions.FindActionMap("UI", true);

        pauseActionGameplay = playerMap.FindAction(pauseActionName, true);
        pauseActionUI = uiMap.FindAction(pauseActionName, true); // o "PauseUI"

        pauseActionGameplay.Enable();
        pauseActionUI.Enable();

        pauseActionGameplay.performed += OnPausePressed;
        pauseActionUI.performed += OnPausePressed;
        
    }

    private void OnDisable()
    {
        if (pauseActionGameplay != null)
        {
            pauseActionGameplay.performed -= OnPausePressed;
            pauseActionGameplay.Disable();
        }
        if (pauseActionUI != null)
        {
            pauseActionUI.performed -= OnPausePressed;
            pauseActionUI.Disable();
        }
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (pauseHandledThisFrame) return; // blocca doppia chiamata
        pauseHandledThisFrame = true;
        
        if (isPaused)
        {
            Debug.Log("Resume Pressed");
            ResumeGame();
        }
        else
        {
            Debug.Log("Pause Pressed");
            PauseGame();
        }
            
    }
    
    void Start()
    {
        eventSystem = EventSystem.current;

        gameplayMap = inputAsset.FindActionMap(gameplayMapName, true);
        uiMap = inputAsset.FindActionMap(uiMapName, true);

        navigateAction = uiMap.FindAction("Navigate", true);
        submitAction = uiMap.FindAction("Submit", true);

        pauseCanvas.SetActive(false);
        pointerMover = pointer.GetComponent<YPositionMover>();
        loadTmpTexts();//carica i tmpControllers

    }

    //take input from array
    void Update()
    {
        /*if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused ) ResumeGame();
            else PauseGame();
        }*/
        //per evitare doppio input in un solo frame
        pauseHandledThisFrame = false;
        
        if (!isPaused) return;

        if (Time.unscaledTime - lastNavigationTime > navigationCooldown)
        {
            Vector2 nav = navigateAction.ReadValue<Vector2>();

            if (nav.y > 0.5f)
            {
                ChangeSelection(-1);
                lastNavigationTime = Time.unscaledTime;
            }
            else if (nav.y < -0.5f)
            {
                ChangeSelection(1);
                lastNavigationTime = Time.unscaledTime;
            }
        }

        if (submitAction.triggered)
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    void ChangeSelection(int direction)
    {
        
        if (buttons.Length == 0) return;

        // Diminuisci font dell'attuale selezionato
        buttonTextControllers[selectedIndex].SetFontMinSize();

        // Calcola nuovo indice in modo sicuro
        selectedIndex += direction;

        if (selectedIndex < 0)
            selectedIndex = buttons.Length - 1;
        else if (selectedIndex >= buttons.Length)
            selectedIndex = 0;

        // Applica effetti grafici e spostamento
        PositionAtIndex(selectedIndex);
        buttonTextControllers[selectedIndex].SetFontMaxSize();

        //if(buttons[selectedIndex] == null)
            //Debug.Log("VUOTO");
        // Imposta il bottone selezionato anche per EventSystem (utile per tastiera)
       eventSystem.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        gameplayMap.Disable();
        uiMap.Enable();

        pauseCanvas.SetActive(true);
        selectedIndex = 0;
         // oppure EnablePointer();
        
        //pointer.transform.position = buttons[selectedIndex].transform.position;
        buttonTextControllers[selectedIndex].SetFontMaxSize();

        isPaused = true;
    }

    public void ResumeGame()
    {
        
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        gameplayMap.Enable();
        uiMap.Disable();
        pointer.SetActive(true);
        pointerMover.SetYPositionByIndex(selectedIndex);
        buttonTextControllers[selectedIndex].SetFontMinSize();
        isPaused = false;
    }

    
    public void EnablePointer() => pointer.SetActive(true);
    
    
    public void PositionAtIndex(int index)
    {
        for (int i = 0; i < buttonTextControllers.Length; i++)
        {
            if(i!=index)
                buttonTextControllers[i].SetFontMinSize();
        }
        
        if (index > buttons.Length - 1 || index < 0)
        {
            return;
        }
        else
        {
            selectedIndex = index;
        }

        pointer.SetActive(true);
        pointerMover.SetYPositionByIndex(index);
    }

    public void DisablePointer()
    {
        for (int i = 0; i < buttonTextControllers.Length; i++)
        {
            buttonTextControllers[i].SetFontMinSize();
        }
        pointer.SetActive(false);
    }
    
    void loadTmpTexts()
    {
        // Checks if the buttons array has been assigned in the Inspector.
        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogWarning("L'array 'buttons' non è stato impostato o è vuoto! " +
                             "Impossibile estrarre i riferimenti TextMeshPro. Si prega di riempirlo nell'Inspector.", this);
            return;
        }

        // Initializes the buttonTexts array with the same size as the input 'buttons' array.
        buttonTextControllers = new FontSizeController[buttons.Length];

        // Iterate through each button in the 'buttons' array.
        for (int i = 0; i < buttons.Length; i++)
        {
            GameObject currentButton = buttons[i].gameObject;

            if (currentButton != null)
            {

                // Find the TextMeshProUGUI component among the children of the current button.
                // GetComponentInChildren<T>() searches in the current GameObject and all its children.
                FontSizeController tmpChildController = currentButton.GetComponentInChildren<FontSizeController>();

                if (tmpChildController != null)
                {
                    buttonTextControllers[i] = tmpChildController; // Assegna il riferimento TextMeshPro trovato.
                }
                else
                {
                    Debug.LogWarning($"Nessun componente TextMeshPro (o TMP_Text) trovato come figlio di '{currentButton.name}' all'indice {i}. " +
                                     "Assicurati che il tuo TextMeshPro sia un figlio del bottone.", currentButton);
                }
            }
            else
            {
                Debug.LogWarning($"Il GameObject del bottone all'indice {i} è nullo nell'array 'buttons'. Saltando questo elemento.", this);
            }
        }

    }
}
