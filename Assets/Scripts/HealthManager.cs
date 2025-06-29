using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    // always work in progress
    // -- please edit this class if you have any ideas to improve the code -- 
    // p.s. remember to comment the code 

    // this class is designed to manage every living entity ( player / enemies in this case )

    private Material startMaterial;
    [SerializeField] private Material blinkMaterial;

    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] ParticleSystem ps;
    [SerializeField] public float maxHealth = 100;
    private int maxBlinks = 2;
    public float currentHealth;

    public float CurrentHealth =>
        currentHealth; // <-- just added, got some errors on character movement, i don't remember if i broke something lol

    private void Start()
    {
        startMaterial = skinnedMeshRenderer.material;
        currentHealth = maxHealth;
    }

    IEnumerator Blink()
    {
        float blinks = 0;
        bool flip = false;

        while (blinks < maxBlinks)
        {
            if (flip)
            {
                blinks += 1;
               
                skinnedMeshRenderer.material = startMaterial;
            }
            else
            {
                skinnedMeshRenderer.material = blinkMaterial;
            }
            flip = !flip;

            yield return new WaitForSeconds(0.1f);
        }

        skinnedMeshRenderer.material = startMaterial;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        StopCoroutine(Blink());
        StartCoroutine(Blink());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("GameOverScene");
        }
        else
        {
            if (ps)
            {
                ps.Play();
                Invoke(nameof(Disable), ps.main.duration);
            }
            else
            {
                Disable();
            }
        }
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}