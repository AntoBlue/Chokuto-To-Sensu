using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject owner;
    
    public GameObject Owner {get => owner; set => owner = value; }

    private void OnTriggerEnter(Collider other)
    {
        //hurt anything with health except the player
        //tag player is there for statue (it does not have the player as its owner)
        if (other.gameObject != owner && !CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            var health = other.gameObject.GetComponent<HealthManager>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}