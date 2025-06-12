using TMPro;
using UnityEngine;

public class MenuPositioner : MonoBehaviour
{
    [SerializeField]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject[] referenceObjects;
    private int positionIndex = 0;
    [SerializeField] private GameObject pointer;
    private YPositionMover pointerMover;
    // This is the array that will contain the TextMeshPro references of the buttons.
    private FontSizeController[] buttonTextControllers;
    void Start()
    {
        pointerMover = pointer.GetComponent<YPositionMover>();
        loadTmpTexts();
    }

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
            GameObject currentButton = referenceObjects[i];

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
    void positionUp()
    {
        buttonTextControllers[positionIndex].SetFontMinSize();
        if (positionIndex == 0)
        {
            positionIndex = referenceObjects.Length - 1;
        }
        else
        {
            positionIndex--;
        }
        pointer.SetActive(true);
        pointerMover.SetYPositionByIndex(positionIndex);
        buttonTextControllers[positionIndex].SetFontMaxSize();
            
    }
    
    void positionDown()
    {   buttonTextControllers[positionIndex].SetFontMinSize();
        if (positionIndex == referenceObjects.Length - 1)
        {
            positionIndex = 0;
        }
        else
        {
            positionIndex++;
        }
        pointer.SetActive(true);
        pointerMover.SetYPositionByIndex(positionIndex);
        buttonTextControllers[positionIndex].SetFontMaxSize();
    }
    
    //Here we dont have buttonTextControllers settings couse is used jet on Mouse Event Trigghers
    public void PositionAtIndex(int index)
    {
        if (index > referenceObjects.Length - 1 || index < 0)
        {
            return;
        }
        else
        {
            positionIndex = index;
        }

        pointer.SetActive(true);
        pointerMover.SetYPositionByIndex(positionIndex);
    }

    public void DisablePointer()
    {
        pointer.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        //Inserire input
    }
}
