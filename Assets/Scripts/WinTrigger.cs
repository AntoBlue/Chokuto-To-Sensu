using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke(nameof(LoadWinScreen), 1f);
           
        }
    }

    private void LoadWinScreen()
    {
        SceneManager.LoadScene("WinScene");
    }
}
