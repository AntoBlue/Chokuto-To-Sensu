using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // always work in progress
    // -- please edit this class if you have any ideas to improve the code -- 
    // p.s. remember to comment the code 
    
                         // this class is designed to manage every living entity ( player / enemies in this case )
                         
    
    [SerializeField] private  int maxHealth = 100;
    private  int currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        
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