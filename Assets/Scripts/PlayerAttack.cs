using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    //projectile
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject UpgradeProjectile;
    [SerializeField] GameObject MeleeAttack;
    [SerializeField] GameObject ChargeMeleeAttack;
    [SerializeField] GameObject Statue;
    [SerializeField] private Animator animator;

    private float ProjectileSpeed;
    private float UpgradeProjectileSpeed;

    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform statueSpawnPoint;


    public bool HasProjectile;
    public bool HasUpgradeProjectile;
    public bool HasStatue;


    float chargeTimer;
    private bool pressingMelee = false;
    private Color defaultColor;

    [SerializeField] int MeleeDamage;
    [SerializeField] int ChargeMeleeDamage;

    [SerializeField] float StatueCooldown;
    [SerializeField] private float meleeCooldown = 0.4f;
    [SerializeField] private float distanceAttackCooldown = 0.25f;
    [SerializeField] private float chargeTime = 2f;
    private float statueCooldownRemaining = 0f;
    private bool StatueActive;

    private bool cooldown;
    
    //input mapping
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player";

    private InputAction attackAction;
    private InputAction attackDistanceAction;
    private InputAction statueSpawnAction;
    private bool spawnStatueNextFrame = false;
    
    [SerializeField] private CharacterMovement characterMovement;


    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip meleeClip;
    [SerializeField] private AudioClip chargeClip;
    [SerializeField] private AudioClip throwClip;
    [SerializeField] private AudioClip castClip;
    [Range(0f, 100f)]
    [SerializeField] private float audioVolume = 100f;

    [Header("UI")]
    [SerializeField] private Image statueCooldownImage;

    private Action<InputAction.CallbackContext> onAttackStarted;
    private Action<InputAction.CallbackContext> onAttackCanceled;
    private Action<InputAction.CallbackContext> onAttackDistancePerformed;
    private Action<InputAction.CallbackContext> onStatueSpawnPerformed;
    
    private void OnEnable()
    {
        var actionMap = inputActions.FindActionMap(actionMapName);

        attackAction = actionMap.FindAction("Attack");
        attackDistanceAction = actionMap.FindAction("AttackDistance");
        statueSpawnAction = actionMap.FindAction("StatueSpawn");

        attackAction.Enable();
        attackDistanceAction.Enable();
        statueSpawnAction.Enable();

        onAttackStarted = ctx => HandleMeleeStart();
        onAttackCanceled = ctx => HandleMeleeRelease();
        onAttackDistancePerformed = ctx => HandleProjectileAttack();
        onStatueSpawnPerformed = ctx => HandleStatueSpawn();

        attackAction.started += onAttackStarted;
        attackAction.canceled += onAttackCanceled;
        attackDistanceAction.performed += onAttackDistancePerformed;
        statueSpawnAction.performed += onStatueSpawnPerformed;
    }

    private void OnDisable()
    {
        attackAction.started -= onAttackStarted;
        attackAction.canceled -= onAttackCanceled;
        attackDistanceAction.performed -= onAttackDistancePerformed;
        statueSpawnAction.performed -= onStatueSpawnPerformed;
    }
    
    private void HandleProjectileAttack()
    {
        if (cooldown) return;

        GameObject bullet = null;

        if (HasUpgradeProjectile)
            bullet = ObjectPool.SharedInstance.GetPooledObject2();
        else if (HasProjectile)
            bullet = ObjectPool.SharedInstance.GetPooledObject();

        if (bullet != null)
        {
            //ranged attack!
            animator.SetTrigger("Range");
            audioSource.PlayOneShot(throwClip, 3f);
            bullet.GetComponent<HasOwner>().Owner = gameObject;
            bullet.transform.position = projectileSpawnPoint.position;
            bullet.SetActive(true);
            
            bool isFacingRight = characterMovement.IsFacingRight;
            bullet.GetComponent<PlayerProjectile>().Activate(isFacingRight ? 1 : -1);
            
            cooldown = true;
            Invoke(nameof(EndCooldown), distanceAttackCooldown);
        }
    }

    private void HandleMeleeStart()
    {
        if (cooldown) return;

        chargeTimer = 0f;
        pressingMelee = true;
    }

    private void HandleMeleeRelease()
    {
        if (!pressingMelee || cooldown) return;
        
        
        if (chargeTimer >= chargeTime)
        {
           animator.SetTrigger("Heavy"); // animazione attacco pesante
           audioSource.PlayOneShot(chargeClip, 1f);
           animator.SetBool("FullCharged", false);
        }
        else
        {
           animator.SetTrigger("Melee"); // animazione attacco normale
           audioSource.PlayOneShot(meleeClip, 1f);
           MeleeAttack.SetActive(true);
           Invoke(nameof(DeactivateMelee), 0.3f);
        }
        

        pressingMelee = false;
        chargeTimer = 0f;

        cooldown = true;
        Invoke(nameof(EndCooldown), meleeCooldown);
    }

    //Called by Animator Event
    private void HeavyAttackEvent()
    {
        ChargeMeleeAttack.SetActive(true);
        Invoke(nameof(DeactivateMelee), 0.2f);
    }

    private void HandleStatueSpawn()
    {
        if (StatueActive || !HasStatue) return;
        spawnStatueNextFrame = true;
        animator.SetTrigger("Cast");
        audioSource.PlayOneShot(castClip, 1f);
    }
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.volume = audioVolume / 100f;

        characterMovement = GetComponent<CharacterMovement>();
        MeleeAttack.GetComponent<HasOwner>().Owner = gameObject;
        ChargeMeleeAttack.GetComponent<HasOwner>().Owner = gameObject;
        Instantiate(Statue, statueSpawnPoint, statueSpawnPoint);
        Statue.SetActive(false);
    }   

    // Update is called once per frame
    void DeactivateMelee()
    {
        MeleeAttack.SetActive(false);
        ChargeMeleeAttack.SetActive(false);
    }

    void DeactivateStatue()
    {
        Statue.SetActive(false);
        StatueActive = false;
    }

    void EndCooldown()
    {
        cooldown = false;
    }

    void Update()
    {
        audioSource.volume = audioVolume / 100f;

        if (pressingMelee)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer >= chargeTime)
            {
                // Visual feedback, se vuoi
                // gameObject.GetComponent<Renderer>().material.color = Color.red;
                animator.SetBool("FullCharged", true);
            }
        }

        //Statue UI
        if (statueCooldownRemaining > 0f)
        {
            statueCooldownRemaining -= Time.deltaTime;
            statueCooldownImage.fillAmount = statueCooldownRemaining / StatueCooldown;
        }
        else
        {
            statueCooldownImage.fillAmount = 0f;
        }

    }

  /*  private void FixedUpdate()
    {
        //determines the direction the player is facing, then gives the same direction to the projectile
        if (Input.GetAxis("Horizontal") > 0)
        {
            facingRight = true;
            facingLeft = false;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            facingRight= false;
            facingLeft = true;
        }
    }*/
    private void LateUpdate()
    {
        if (spawnStatueNextFrame)
        {
            Statue.GetComponent<HasOwner>().Owner = gameObject;
            Statue.transform.position = statueSpawnPoint.position;
            Statue.SetActive(true);
            StatueActive = true;
            statueCooldownRemaining = StatueCooldown;
            Invoke(nameof(DeactivateStatue), StatueCooldown);
            Statue.GetComponent<StatueAttack>().ResetColor();
            spawnStatueNextFrame = false;
        }
    }
}
