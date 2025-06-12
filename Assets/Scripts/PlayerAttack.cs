using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //projectile
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject UpgradeProjectile;

    [SerializeField] private float ProjectileSpeed;
    [SerializeField] private float UpgradeProjectileSpeed;

    [SerializeField] private Transform projectileSpawnPoint; 

    public bool HasProjectile;
    private bool HasUpgradeProjectile;


    //private void OnCollisionEnter(Collision other)
    //{
    //    //if (other.gameObject.GetComponent<FireBulletsAtTarget>()) return;

    //    Destroy(gameObject);
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HasProjectile)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                var bullet = Instantiate(Projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                Projectile.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.up * ProjectileSpeed;
            }
        }
        

        if (Input.GetKeyDown(KeyCode.L))
        {

        }
    }
}
