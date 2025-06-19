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

    [SerializeField] int MeleeDamage;
    [SerializeField] int ChargeMeleeDamage;

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
        if (HasProjectile && HasUpgradeProjectile == false)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {       
                GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
                if (bullet != null)
                {
                    bullet.transform.position = projectileSpawnPoint.transform.position;
                    bullet.SetActive(true);
                    bullet.GetComponent<PlayerProjectile>().Activate(gameObject);
                    
                    //Vector3 direction = gameObject.transform.forward;
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
                    bullet.SetActive(true);
                    bullet.GetComponent<PlayerProjectile>().Activate(gameObject);

                    //Vector3 direction = gameObject.transform.localScale;
                }

                bullet.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.up * UpgradeProjectileSpeed;
            }
        }

        //Charge Melee Attack
        if (Input.GetKey(KeyCode.L))
        {
            chargeTimer += Time.deltaTime;
            pressingMelee = true;

            //Visual cue: turn player red when attack is fully charged
            if (chargeTimer >= 3)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
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
