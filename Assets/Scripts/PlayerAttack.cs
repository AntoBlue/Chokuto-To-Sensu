using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    void DeactivateMelee()
    {
        MeleeAttack.SetActive(false);
    }

    void Update()
    {


        //GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
        //if (bullet != null)
        //{
        //    bullet.transform.position = projectileSpawnPoint.transform.position;
        //    bullet.transform.rotation = projectileSpawnPoint.transform.rotation;
        //    bullet.SetActive(true);
        //}


        if (HasProjectile && HasUpgradeProjectile == false)
        {


            if (Input.GetKeyDown(KeyCode.P))
            {
                //var bullet = Instantiate(Projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                
                GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
                if (bullet != null)
                {
                    bullet.transform.position = projectileSpawnPoint.transform.position;
                    //bullet.transform.rotation = new Quaternion (0, 180, 0);
                    bullet.SetActive(true);
                    bullet.GetComponent<PlayerProjectile>().Activate(gameObject);
                    

                    //Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                    Vector3 direction = gameObject.transform.localScale;
                    //bullet.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x, 0, 0) * (bullet. * 10));
                }

                bullet.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.up * ProjectileSpeed;
            }

        }

        if(HasUpgradeProjectile)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameObject bullet = ObjectPool.SharedInstance.GetPooledObject2();
                if (bullet != null)
                {
                    bullet.transform.position = projectileSpawnPoint.transform.position;
                    //bullet.transform.rotation = (projectileSpawnPoint.transform.rotation);
                    bullet.SetActive(true);
                    bullet.GetComponent<PlayerProjectile>().Activate(gameObject);


                    //Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                    Vector3 direction = gameObject.transform.localScale;
                    //bullet.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x, 0, 0) * (bullet. * 10));
                }

                bullet.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.up * UpgradeProjectileSpeed;
            }
        }
        

        if (Input.GetKeyDown(KeyCode.L))
        {
            MeleeAttack.SetActive(true);
            Invoke("DeactivateMelee", 0.25f);
        }


    }
}
