using Unity.VisualScripting;
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

    private bool facingRight;
    private bool facingLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Color defaultColor = gameObject.GetComponent<Renderer>().material.color;
    }   

    // Update is called once per frame
    void DeactivateMelee()
    {
        MeleeAttack.SetActive(false);
        ChargeMeleeAttack.SetActive(false);
    }

    void Update()
    {
        //determines the direction the player is facing, then gives the same direction to the projectile
        if (Input.GetAxis("Horizontal") > 0)
        {
            facingRight = true;
            facingLeft = false;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            facingRight= false;
            facingLeft = true;
        }

        //shooting
        if (HasProjectile && HasUpgradeProjectile == false)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {       
                GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
                if (bullet != null && facingRight == true)
                {
                    bullet.transform.position = projectileSpawnPoint.transform.position;
                    bullet.SetActive(true);
                    bullet.GetComponent<PlayerProjectile>().Activate(gameObject, 1);
                }

                if (bullet != null && facingLeft == true)
                {
                    bullet.transform.position = projectileSpawnPoint.transform.position;
                    bullet.SetActive(true);
                    bullet.GetComponent<PlayerProjectile>().Activate(gameObject, -1);
                }

                //bullet.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.up * ProjectileSpeed;
                Vector3 shootDirection = transform.forward;
                shootDirection.y = 0; // ignora l'inclinazione in salto
                shootDirection.Normalize();
                bullet.GetComponent<Rigidbody>().linearVelocity = shootDirection * ProjectileSpeed;
            }

        }

        if(HasUpgradeProjectile)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameObject bullet = ObjectPool.SharedInstance.GetPooledObject2();
                if (bullet != null && facingRight == true)
                {
                    bullet.transform.position = projectileSpawnPoint.transform.position;
                    bullet.SetActive(true);
                    bullet.GetComponent<PlayerProjectile>().Activate(gameObject, 1);
                }

                if (bullet != null && facingLeft == true)
                {
                    bullet.transform.position = projectileSpawnPoint.transform.position;
                    bullet.SetActive(true);
                    bullet.GetComponent<PlayerProjectile>().Activate(gameObject, -1);
                }

                Vector3 shootDirection = transform.forward;
                shootDirection.y = 0;
                shootDirection.Normalize();
                bullet.GetComponent<Rigidbody>().linearVelocity = shootDirection * UpgradeProjectileSpeed;
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
                //gameObject.GetComponent<Renderer>().material.color = Color.red;
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
                //gameObject.GetComponent<Renderer>().material.color = defaultColor;
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
