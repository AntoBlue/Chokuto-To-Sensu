using UnityEngine;


public class SP_SpawnStatue : MonoBehaviour

{
    [SerializeField] private GameObject statuePrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float cooldownTime = 20f; // 10f == 1 second

    private float cooldownTimer = 0f;

    void Start()
    {
        statuePrefab.SetActive(false);
    }

    void Shoot()
    {
        Debug.Log("S H O O O O O O T I N G");

        GameObject newStatue = Instantiate(statuePrefab, spawnPosition.position, spawnPosition.rotation);
        newStatue.layer = LayerMask.NameToLayer("Ground");
        newStatue.SetActive(true);
        Destroy(newStatue, 3f);
        cooldownTimer = cooldownTime; // reset the cooldown timer
    }

    void Update()
    {
        // decrease the cooldown timer
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= cooldownTime * Time.deltaTime;
           // Debug.Log(cooldownTimer);

        }

        // shooting if cooldown <= 0f
        if (Input.GetKeyDown(KeyCode.K) && cooldownTimer <= 0f)
        {
            Debug.Log("--------- Ready to shoot. ---------");

            Shoot();
        }
        
    }
}