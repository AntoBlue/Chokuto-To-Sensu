using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(HasOwner))]
public class AreaDamage : MonoBehaviour
{
    // this class is designed for area attacks -- will define a radius as the area

    [SerializeField] private float damageRadius = 3f;
    [SerializeField] private int damageAmount = 50;
    [SerializeField] private LayerMask damageLayer;

    private bool hasCollided = false;
    
    private HasOwner hasOwner;
    void Awake()
    {
        hasOwner = GetComponent<HasOwner>();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!hasCollided)
        {
            hasCollided = true;
            MakeAreaDamage();
            Destroy(gameObject, 1.5f); // Destroy after short delay

            Debug.Log("Area hasCollided");
        }
    }

    private void MakeAreaDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius, damageLayer);

        foreach (var hit in hits)
        {
            if (hasOwner.Owner != hit.gameObject && !hasOwner.Owner.CompareTag(hit.gameObject.tag))
            {
                var health = hit.GetComponent<HealthManager>();
                if (health != null)
                {
                    health.TakeDamage(damageAmount);
                    Debug.Log("Damage taken");
                }
            }
        }
    }
}