using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // always work in progress
    // -- please edit this class if you have any ideas to improve the code -- 
    // p.s. remember to comment the code 
    
                         // this class is designed to manage every living entity ( player / enemies in this case )
                         
    
    [SerializeField] private  int maxHealth = 100;
    private  int currentHealth;
    
    [SerializeField] private  int points = 0;
    [SerializeField] private  int damage = 10; // this var is for testing only, do not use it in the actual game

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        points = points - damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Destroy(gameObject); // died lol
        gameObject.SetActive(false);
    }
}