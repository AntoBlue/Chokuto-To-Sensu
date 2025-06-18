using UnityEngine;

public class ProjectilePowerUp : MonoBehaviour
{
    //public GameObject pickupEffect;
    private GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    //gives appropriate power up to player
    //to do: change tag system to enum
    void Pickup()
    {
        if(gameObject.CompareTag("Projectile_PU"))
        {
            PlayerAttack playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
            playerAttack.HasProjectile = true;
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("ProjectileUpgrade_PU"))
        {
            PlayerAttack playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
            playerAttack.HasUpgradeProjectile = true;
            Destroy(gameObject);
        }
    }
}
