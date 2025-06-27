using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // always work in progress
    // -- please edit this class if you have any ideas to improve the code -- 
    // p.s. remember to comment the code 
    
                         // this class is designed to manage every living entity ( player / enemies in this case )
                         
    
    [SerializeField] public float maxHealth = 100;
    public float currentHealth;
    
    public int CurrentHealth => currentHealth; // <-- just added, got some errors on character movement, i don't remember if i broke something lol
    
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

    public void Die()
    {
        //Destroy(gameObject); // died lol
        gameObject.SetActive(false);
    }
}