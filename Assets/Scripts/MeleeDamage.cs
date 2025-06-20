using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject owner;
    
    public GameObject Owner {get => owner; set => owner = value; }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject != owner)
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