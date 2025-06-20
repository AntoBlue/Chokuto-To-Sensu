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
        //Vector3 direction = Player.transform.forward;
    }

    //de-spawn projectile when needed
    void Deactivate()
    {    
        gameObject.SetActive(false);
    }

    //spawn projectile when PlayerAttack calls it
    public void Activate(GameObject projectile, float bulletDirection)
    {
        //float bulletDirection;
        //float lastHorizontal = 

        //get movement imput, if idle, get last imput
        //if (Input.GetAxis("Horizontal") > 0)
        //{
            rb.AddForce(new Vector3(bulletDirection, 0, 0) * (_speed * 10));
        //}

        //if (Input.GetAxis("Horizontal") < 0)
        //{
        //    rb.AddForce(new Vector3(-1, 0, 0) * (_speed * 10));
        //}

        ////currently doesn't work
        //else
        //{
        //    rb.AddForce(new Vector3(lastHorizontal, 0, 0) * (_speed * 10));
        //}

            Invoke("Deactivate", 3);
    }

    //damage enemy and set projectile to inactive
    void OnCollisionEnter(Collision other)
    {
        //don't do anything if projectile collides with player
        if (!other.gameObject.CompareTag("Player"))
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
        transform.Rotate(rotationSpeed, 0, 0 * Time.deltaTime);
    }


}
