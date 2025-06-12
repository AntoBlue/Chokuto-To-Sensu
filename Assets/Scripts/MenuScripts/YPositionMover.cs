using UnityEngine;

public class YPositionMover : MonoBehaviour
{
    [SerializeField]
    // This field is an array of GameObject references, which will serve as Y "positions".
    private GameObject[] referenceObjects;
    private float[] yPositions;

    void Start()
    {
        
        if (referenceObjects == null || referenceObjects.Length == 0)
        {
            Debug.LogError("L'array 'referenceObjects' non è stato impostato o è vuoto! " +
                           "Impossibile pre-caricare le posizioni Y. Si prega di riempirlo nell'Inspector.");
            // Cannot pre-cache Y positions. Please populate it in the Inspector.
            return;
        }
        
        // Initialize the cache array with the same size as the reference array.
        yPositions = new float[referenceObjects.Length];
        
        for (int i = 0; i < referenceObjects.Length; i++)
        {
            if (referenceObjects[i] != null)
            {
                yPositions[i] = referenceObjects[i].transform.position.y;
            }
            else
            {
                Debug.LogWarning($"Il GameObject di riferimento all'indice {i} è nullo in 'referenceObjects'! " +
                                 $"Questa posizione Y non verrà pre-caricata correttamente.");
                yPositions[i] = 0; // Assegna un valore predefinito o gestisci come preferisci.
            }
        }
    }
    /// Moves the object to the Y position specified by the index in the 'yPositions' array.
    /// The object's X and Z positions will remain unchanged.
    /// If the index is invalid, a warning will be printed and the object will not move.
    /// <param name="index">The index in the 'yPositions' array that contains the desired Y.</param>
    public void SetYPositionByIndex(int index)
    {
        // Checks if the Y positions array has been assigned in the Inspector or is empty.
        if (yPositions == null || yPositions.Length == 0)
        {
            Debug.LogError("L'array 'yPositions' non è stato impostato o è vuoto! " +
                           "Si prega di riempirlo nell'Inspector.");
            return;
        }
        // Checks if the provided index is valid (within the array bounds).
        if (index < 0 || index >= yPositions.Length)
        {
            Debug.LogWarning($"Indice non valido: {index}. L'indice deve essere compreso tra 0 e {yPositions.Length - 1}. " +
                             $"L'oggetto non si sposterà.");
            return; 
        }
        // Get the desired Y position from the array.
        float targetY = yPositions[index];
        // Create a new Vector3 position keeping the current X and Z, but updating the Y.
        Vector3 newPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = newPosition;

        Debug.Log($"Oggetto spostato alla posizione Y: {newPosition.y} (indice: {index})");
        // Object moved to Y position: {newPosition.y} (index: {index})
    }
}
