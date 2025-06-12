using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BackToSelector : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player"; // o il nome corretto del tuo action map
    [SerializeField] private string actionName = "BackInspector"; // il nome esatto della tua InputAction
    [SerializeField] private SceneAsset backScene;
    private string sceneToLoad;

    private InputAction backAction;

    private void Start()
    {
        sceneToLoad = backScene.name;
    }
 
    private void OnEnable()
    {
        var actionMap = inputActions.FindActionMap(actionMapName, true);
        backAction = actionMap.FindAction(actionName, true);
        backAction.Enable();
        backAction.performed += OnBackPressed;
    }

    private void OnDisable()
    {
        backAction.performed -= OnBackPressed;
        backAction.Disable();
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
