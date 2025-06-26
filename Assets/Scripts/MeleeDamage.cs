using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(HasOwner))]
public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    
    private HasOwner hasOwner;
    void Awake()
    {
        hasOwner = GetComponent<HasOwner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject != hasOwner.Owner)
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