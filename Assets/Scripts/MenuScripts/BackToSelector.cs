using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BackToSelector : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player"; // o il nome corretto del tuo action map
    [SerializeField] private string actionName = "BackSelector"; // il nome esatto della tua InputAction
    [SerializeField] private string backSceneName;
    private string sceneToLoad;

    private InputAction backAction;
    private InputAction backActionUI;

    private void Start()
    {
        sceneToLoad = backSceneName;
    }
 
    private void OnEnable()
    {
        var actionMap = inputActions.FindActionMap(actionMapName, true);
        backAction = actionMap.FindAction(actionName, true);
        backAction.Enable();
        backAction.performed += OnBackPressed;
        
        var uiMap = inputActions.FindActionMap("UI", true);
        backActionUI = uiMap.FindAction(actionName, true);
        backActionUI.Enable();
        backActionUI.performed += OnBackPressed;
    }

    private void OnDisable()
    {
        backAction.performed -= OnBackPressed;
        backAction.Disable();
        
        if (backActionUI != null)
        {
            backActionUI.performed -= OnBackPressed;
            backActionUI.Disable();
        }
    }

    public void OnBackPressed(InputAction.CallbackContext context)
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Nessuna scena specificata!");
        }
    }
}
