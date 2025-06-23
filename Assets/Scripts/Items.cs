using UnityEngine;

public class Items : MonoBehaviour
{
    
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float floatAmplitude = 0.25f; // quanto si alza/abbassa
    [SerializeField] private float floatFrequency = 1f;     // velocità dell'oscillazione

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Rotazione attorno all'asse Y
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        // Movimento su/giù sinusoidale
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
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
