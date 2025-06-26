using JetBrains.Annotations;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private GameObject owner;

    public GameObject Owner
    {
        get => owner;
        set => owner = value;
    }

    [SerializeField] private float life = 3;
    [SerializeField] private float _speed;
    [SerializeField] private int damage;
    [SerializeField] private int rotationSpeed;
    [SerializeField] GameObject Player;
    [SerializeField] Rigidbody rb;

    private Vector3 direction;
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
        if (Owner != other.gameObject)
        {
            var health = other.gameObject.GetComponent<HealthManager>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            Deactivate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //rotation animation
        rb.MovePosition(transform.position + direction * (_speed * Time.deltaTime));
        transform.Rotate(rotationSpeed, 0, 0 * Time.deltaTime);
    }
}