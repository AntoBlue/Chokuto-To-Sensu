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

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = Player.GetComponent<HealthManager>().maxHealth;
        bar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Player.GetComponent<HealthManager>().currentHealth;

        Debug.Log(currentHealth / maxHealth);
        bar.fillAmount = (currentHealth / maxHealth);
        Debug.Log(bar.fillAmount);
    }
}
