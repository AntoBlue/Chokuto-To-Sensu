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
        Debug.Log("PreCheck");
        if (other.gameObject != hasOwner.Owner && !hasOwner.Owner.CompareTag(other.gameObject.tag))
        {
            Debug.Log("AfterCheck");
            var health = other.gameObject.GetComponent<HealthManager>();
            if (health != null)
            {
                Debug.Log("Damage");
                health.TakeDamage(damage);
            }
        }
    }
}