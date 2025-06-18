using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAttack : MonoBehaviour
{
    //projectile
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject UpgradeProjectile;
    [SerializeField] GameObject MeleeAttack;
    [SerializeField] GameObject ChargeMeleeAttack;

    private float ProjectileSpeed;
    private float UpgradeProjectileSpeed;


    [SerializeField] private Transform projectileSpawnPoint; 

    public bool HasProjectile;
    public bool HasUpgradeProjectile;

    float chargeTimer;
    private bool pressingMelee = false;
    private Color defaultColor;

    //private void OnCollisionEnter(Collision other)
    //{
    //    //if (other.gameObject.GetComponent<FireBulletsAtTarget>()) return;

    //    Destroy(gameObject);
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color defaultColor = gameObject.GetComponent<Renderer>().material.color;
    }   

    // Update is called once per frame
    void DeactivateMelee()
    {
        MeleeAttack.SetActive(false);
        ChargeMeleeAttack.SetActive(false);
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

        //Charge Melee Attack
        if (Input.GetKey(KeyCode.L))
        {
            chargeTimer += 0.01f;
            //gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);
            pressingMelee = true;
            //gameObject.GetComponent<Renderer>().material.color = new Color(chargeTimer, 0, 0);

            //Visual cue: turn player red when attack is fully charged
            if (chargeTimer >= 3)
            {
                gameObject.GetComponent<Renderer>().material.color = new Color(chargeTimer, 0, 0);
            }

        }

        if (Input.GetKeyUp(KeyCode.L) && pressingMelee == true)
        {

            //big attack if button is pressed for 3 or more seconds
            if(chargeTimer >= 3)
            {
                ChargeMeleeAttack.SetActive(true);
                Invoke("DeactivateMelee", 0.35f);
                pressingMelee = false;
                chargeTimer = 0;
                gameObject.GetComponent<Renderer>().material.color = defaultColor;
            }

            //normal attack otherwise
            else
            {
                MeleeAttack.SetActive(true);
                Invoke("DeactivateMelee", 0.25f);
                pressingMelee = false;
                chargeTimer = 0;
            }

        }


    }
}
