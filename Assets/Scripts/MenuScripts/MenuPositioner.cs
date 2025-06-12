using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuPositioner : MonoBehaviour
{
    [SerializeField]
    private Selectable[] referenceObjects;//my buttons
    private EventSystem eventSystem;
    private int positionIndex = -1;
    
    [SerializeField] private GameObject pointer;
    private YPositionMover pointerMover;
    // This is the array that will contain the TextMeshPro references of the buttons.
    private FontSizeController[] buttonTextControllers;
    void Start()
    {
        eventSystem = EventSystem.current; // Ottiene l'istanza corrente dell'EventSystem
        
        if (eventSystem == null)
        {
            Debug.LogError("EventSystem non trovato nella scena! La navigazione UI non funzionerà.", this);
            return;
        }
        pointerMover = pointer.GetComponent<YPositionMover>();
        loadTmpTexts();//carica i tmpControllers
        
        if (referenceObjects != null && referenceObjects.Length > 0)
        {
            // Cerca se c'è già un elemento selezionato dall'EventSystem (es. al primo click del mouse)
            // Search if there's already an element selected by the EventSystem (e.g., on first mouse click)
            GameObject currentlySelected = eventSystem.currentSelectedGameObject;
            if (currentlySelected != null)
            {
                for (int i = 0; i < referenceObjects.Length; i++)
                {
                    if (referenceObjects[i] != null && referenceObjects[i].gameObject == currentlySelected)
                    {
                        positionIndex = i;
                        break;
                    }
                }
            }

            // Se nessun elemento è selezionato o quello selezionato non è nel nostro array,
            // seleziona il primo elemento nell'array 'menuItems' come fallback.
            // If no element is selected or the selected one is not in our array,
            // select the first element in the 'menuItems' array as a fallback.
            if (positionIndex == -1)
            {
                positionIndex = 0;
            }
            
           // SelectMenuItem(positionIndex); // Seleziona l'elemento iniziale
          StartCoroutine(DelayedInitialSelection());
        }
        else
        {
            Debug.LogWarning("Nessun elemento UI assegnato all'array 'menuItems'. La navigazione non funzionerà.");
        }
    }
    
    IEnumerator DelayedInitialSelection()
    {
        yield return null; // aspetta 1 frame

        eventSystem.SetSelectedGameObject(null); // resetta
        eventSystem.SetSelectedGameObject(referenceObjects[0].gameObject); // rilancia il Select
    }
  
  
  //--------------------------------------------------------------------------------------------
void loadTmpTexts()
    {
        // Checks if the buttons array has been assigned in the Inspector.
        if (referenceObjects == null || referenceObjects.Length == 0)
        {
            Debug.LogWarning("L'array 'buttons' non è stato impostato o è vuoto! " +
                             "Impossibile estrarre i riferimenti TextMeshPro. Si prega di riempirlo nell'Inspector.", this);
            return;
        }

        // Initializes the buttonTexts array with the same size as the input 'buttons' array.
        buttonTextControllers = new FontSizeController[referenceObjects.Length];

        // Iterate through each button in the 'buttons' array.
        for (int i = 0; i < referenceObjects.Length; i++)
        {
            GameObject currentButton = referenceObjects[i].gameObject;

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


    //Here we dont have buttonTextControllers settings couse is used jet on Mouse Event Trigghers
    public void PositionAtIndex(int index)
    {
        for (int i = 0; i < buttonTextControllers.Length; i++)
        {
            if(i!=index)
                buttonTextControllers[i].SetFontMinSize();
        }
        
        if (index > referenceObjects.Length - 1 || index < 0)
        {
            return;
        }
        else
        {
            //positionIndex = index;
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
    
    // Update is called once per frame
    void Update()
    {
        //Inserire input
    }
}
