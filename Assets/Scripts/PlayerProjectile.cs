using JetBrains.Annotations;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [SerializeField] private float life = 3;
    [SerializeField] public float _speed;
    [SerializeField] private int damage;
    [SerializeField] private int rotationSpeed;
    [SerializeField] GameObject Player;
    [SerializeField] Rigidbody rb;

    float lastHorizontal;

    private Quaternion playerDirection;
    //private int bulletDirection;
    public void Configure(float speed)
    {
        _speed = speed;
    }

    //de-spawn projectile when needed
    void Deactivate()
    {    
        gameObject.SetActive(false);
    }

    //spawn projectile when PlayerAttack calls it
    public void Activate(GameObject projectile, float bulletDirection)
    {
            rb.AddForce(new Vector3(bulletDirection, 0, 0) * (_speed * 10));
            Invoke("Deactivate", life);
    }

    //damage enemy and set projectile to inactive
    //projectiles use a different layer compared to the player,
    //nothing happens if they collide against it or each other
    void OnCollisionEnter(Collision other)
    {
            var health = other.gameObject.GetComponent<HealthManager>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        //rotation animation
        transform.Rotate(rotationSpeed, 0, 0 * Time.deltaTime);
    }


}
