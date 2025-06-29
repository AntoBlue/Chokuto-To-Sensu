using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //private GameObject healthBar;
    private Image bar;
    private float barLenght;
    [SerializeField] GameObject Player;
    private float maxHealth;
    private float currentHealth;

    private HealthManager playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealth = Player.GetComponent<HealthManager>();
        maxHealth = playerHealth.MaxHealth;
        bar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //Gestire controllo oppure disattivare e mettere animation
        currentHealth = playerHealth.CurrentHealth;
        
        bar.fillAmount = (currentHealth / maxHealth);
    }
}
