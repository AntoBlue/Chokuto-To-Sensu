using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //projectile
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject UpgradeProjectile;
    [SerializeField] GameObject MeleeAttack;

    private float ProjectileSpeed;
    private float UpgradeProjectileSpeed;

    [SerializeField] private Transform projectileSpawnPoint; 

    public bool HasProjectile;
    public bool HasUpgradeProjectile;


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
        if (HasProjectile && HasUpgradeProjectile == false)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                var bullet = Instantiate(Projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                Projectile.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.up * ProjectileSpeed;
            }
        }

        if(HasUpgradeProjectile)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                var bullet = Instantiate(UpgradeProjectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                UpgradeProjectile.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.up * UpgradeProjectileSpeed;
            }
        }
        

        if (Input.GetKeyDown(KeyCode.L))
        {
            var melee = Instantiate(MeleeAttack, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Destroy(melee, 0.5f);
        }
    }
}
