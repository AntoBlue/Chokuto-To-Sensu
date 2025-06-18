using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] public int damage;

    private void OnTriggerEnter(Collider other)
    {
        var health = other.gameObject.GetComponent<HealthManager>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
