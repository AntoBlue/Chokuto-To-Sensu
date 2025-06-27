using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HealthManager>().Die();
            other.GetComponent <HealthManager>().currentHealth = 0;
        }
    }
}
