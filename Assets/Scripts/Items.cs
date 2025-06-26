using UnityEngine;

public class Items : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    //gives appropriate power up to player
    //to do: change tag system to enum
    void Pickup(Collider player)
    {
        if(gameObject.CompareTag("Projectile_PU"))
        {
            player.GetComponent<PlayerAttack>().HasProjectile = true;
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("ProjectileUpgrade_PU"))
        {
            player.GetComponent<PlayerAttack>().HasUpgradeProjectile = true;
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("Key"))
        {
            player.GetComponent<CharacterMovement>().hasKey = true;
            Destroy(gameObject);
        }

        if (gameObject.CompareTag("Statue_PU"))
        {
            player.GetComponent<PlayerAttack>().HasStatue = true;
            Destroy(gameObject);
        }
    }
    
  
}
