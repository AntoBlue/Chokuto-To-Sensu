using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("GameOverScene");
            other.GetComponent<HealthManager>().Die();
            other.GetComponent <HealthManager>().currentHealth = 0;
        }
    }
}
