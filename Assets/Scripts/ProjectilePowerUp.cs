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

    void Pickup()
    {
        //GameObject player = Player.GetComponent<GameObject>();
        //PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
        PlayerAttack playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        playerAttack.HasProjectile = true;
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
