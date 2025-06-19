using TMPro;
using UnityEngine;

public class FontSizeController : MonoBehaviour
{
    // Reference to the TextMeshPro component attached to this GameObject.
    // We use TMP_Text to be compatible with both TextMeshPro - Text (for 3D)
    // and TextMeshPro - UGUI (for UI).
    private TMP_Text tmpText;
    [SerializeField] float maxFontSize=86;
    [SerializeField] float minFontSize=65;
    
    void Start()
    {
        // Try to get the TMP_Text component attached to this GameObject.
        tmpText = GetComponent<TMP_Text>();
        
        // If we don't find the component, we print an error to inform the user.
        if (tmpText == null)
        {
            Debug.LogError("Il componente TextMeshPro non è stato trovato su questo GameObject! " +
                           "Assicurati che lo script sia attaccato a un GameObject con TextMeshPro.", this);
        }
    }

    /// <summary>
    /// Increases the font size of the TextMeshPro to 88.
    /// </summary>
    public void SetFontMaxSize()
    {
        // Checks if the tmpText reference is valid before using it.
        if (tmpText != null)
        {
            tmpText.fontSize = maxFontSize;
            Debug.Log($"Dimensione font impostata a 88 per: {gameObject.name}");
            // Font size set to 88 for: {gameObject.name}
        }
    }

    /// <summary>
    /// Decreases the font size of the TextMeshPro to 60.
    /// </summary>
    public void SetFontMinSize()
    {
        // Checks if the tmpText reference is valid before using it.
        if (tmpText != null)
        {
            tmpText.fontSize = minFontSize;
            Debug.Log($"Dimensione font impostata a 60 per: {gameObject.name}");
            // Font size set to 60 for: {gameObject.name}
        }
    }
}
