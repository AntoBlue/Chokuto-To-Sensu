using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HasOwner))]
public class PlayerProjectile : MonoBehaviour
{
    
    private float life = 2;
    [SerializeField] private float _speed;
    [SerializeField] private int damage;
    [SerializeField] private int rotationSpeed;
    [SerializeField] Rigidbody rb;

    private Vector3 direction;
    
    private HasOwner hasOwner;
    void Awake()
    {
        hasOwner = GetComponent<HasOwner>();
    }
    
    //de-spawn projectile when needed
    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    //spawn projectile when PlayerAttack calls it
    public void Activate(float bulletDirection)
    {
        direction = new Vector3(bulletDirection, 0, 0);
        Invoke("Deactivate", life);
    }

    //damage enemy and set projectile to inactive
    //projectiles use a different layer compared to the player,
    //nothing happens if they collide against it or each other
    void OnCollisionEnter(Collision other)
    {
        if (hasOwner.Owner != other.gameObject && !hasOwner.Owner.CompareTag(other.gameObject.tag))
        {
            var health = other.gameObject.GetComponent<HealthManager>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            Deactivate();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (hasOwner.Owner != other.gameObject)
        {
            if(!hasOwner.Owner.CompareTag(other.gameObject.tag))
            {
                var health = other.gameObject.GetComponent<HealthManager>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }

            Deactivate();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rotation animation
        rb.MovePosition(transform.position + direction * (_speed * Time.fixedDeltaTime));
        transform.Rotate(rotationSpeed, 0, 0 * Time.fixedDeltaTime);
    }
}