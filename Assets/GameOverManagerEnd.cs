using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverManagerEnd : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private Button[] buttons; // You can have multiple buttons
    [SerializeField] private GameObject pointer; // Visual pointer (optional)

    [Header("Input")]
    [SerializeField] private InputActionAsset inputAsset;
    private InputActionMap uiMap;
    private InputAction navigateAction;
    private InputAction submitAction;

    private EventSystem eventSystem;

    private int selectedIndex = 0;
    private bool pointerEnabled = true;
    // This is the array that will contain the TextMeshPro references of the buttons.
    private FontSizeController[] buttonTextControllers;

    void Start()
    {
        loadTmpTexts();
        eventSystem = EventSystem.current;

        gameOverCanvas.SetActive(true);

        uiMap = inputAsset.FindActionMap("UI", true);
        navigateAction = uiMap.FindAction("Navigate", true);
        submitAction = uiMap.FindAction("Submit", true);

        uiMap.Enable();

        // Initially select the first button
        PositionAtIndex(0);
        
        
    }

    void Update()
    {
        // If pointer is enabled, keep it aligned
        if (pointer != null && pointerEnabled)
        {
            pointer.SetActive(true);
            pointer.transform.position= new Vector3( pointer.transform.position.x,buttons[selectedIndex].transform.position.y,pointer.transform.position.z);
            buttonTextControllers[selectedIndex].SetFontMaxSize();
        }

        // If user presses any navigation input, re-enable the pointer
        if (!pointerEnabled && navigateAction.ReadValue<Vector2>() != Vector2.zero)
        {
            PositionAtIndex(selectedIndex);
            pointerEnabled = true;
        }

        // Trigger the button click if Submit is pressed
        if (submitAction.triggered)
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    /// <summary>
    /// Call this to select a button at a specific index (e.g., from mouse hover)
    /// </summary>
    public void PositionAtIndex(int index)
    {
        if (index < 0 || index >= buttons.Length)
        {
            Debug.LogWarning("Button index out of range!");
            return;
        }

        selectedIndex = index;

        if (pointer != null)
        {
            pointer.SetActive(true);
        }

        pointerEnabled = true;

        eventSystem.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }

    /// <summary>
    /// Call this when the mouse exits the button to hide the pointer
    /// </summary>
    public void DisablePointer()
    {
        pointerEnabled = false;

        if (pointer != null)
        {
            pointer.SetActive(false);
        }

        // Optionally clear the selected gameObject so keyboard doesn't affect it
        eventSystem.SetSelectedGameObject(null);
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
