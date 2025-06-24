using UnityEngine;
using UnityEngine.InputSystem;

public enum JumpMode
{
    NoJump = 0,
    SingleJump = 1,
    DoubleJump = 2,
    TripleJump = 3
}

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private  float moveSpeed = 5f;
    [SerializeField] private  float airMoveSpeed = 9f;
    [SerializeField] private  float jumpForce = 7f;
    [SerializeField] private  float gravityMultiplier = 3f;

    [Header("Ground Check")]
    [SerializeField] private  Transform groundCheck;
    [SerializeField] private  float groundCheckRadius = 0.2f;
    [SerializeField] private  LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private bool jumpPressed;
        
    //input system
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player"; 
    [SerializeField] private string jumpActionName = "Jump";
    private InputAction jumpAction;
    [SerializeField] private float ascentGravityMultiplier = 1.5f;  
      
    //Jump buffering
    [SerializeField] private float jumpBufferTime = 0.2f; 
    private float jumpBufferCounter = 0f;
    
    //Double and triple Jump
    [SerializeField] private JumpMode jumpMode = JumpMode.SingleJump; // default: 1 jump
    private int jumpsRemaining;
    
    //Coyote time
    [SerializeField] private float coyoteTime = 0.15f; 
    private float coyoteTimer = 0f;
    
    //double jump less strong
    [SerializeField] private float airJumpForceMultiplier = 0.8f;
    
    //animator
    [SerializeField] private Animator animator;
    private float animationSpeedSmooth = 5f;

    public bool hasKey;
    
    //flip deceleration
    private float previousHorizontalInput = 0f;
    [SerializeField] private float decelerationFactor = 0.85f;
    private float lastNonZeroHorizontalInput = 1f; // default facing right
    
    //Raycast isGrounded
    [Header("Advanced Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float raycastOffset = 0.3f;

    [SerializeField] private PauseManager pauseManager;
    
    private bool IsGrounded()
    {
        Vector3 originCenter = groundCheck.position;
        Vector3 originLeft = originCenter - transform.forward * raycastOffset;
        Vector3 originRight = originCenter + transform.forward * raycastOffset;

        bool centerHit = Physics.Raycast(originCenter, Vector3.down, groundCheckDistance, groundLayer);
        bool leftHit = Physics.Raycast(originLeft, Vector3.down, groundCheckDistance, groundLayer);
        bool rightHit = Physics.Raycast(originRight, Vector3.down, groundCheckDistance, groundLayer);

        if (centerHit || leftHit || rightHit)
        {
            Debug.Log($"isGrounded");
        }
        
        Debug.DrawRay(originCenter, Vector3.down * groundCheckDistance, Color.red);
        Debug.DrawRay(originLeft, Vector3.down * groundCheckDistance, Color.green);
        Debug.DrawRay(originRight, Vector3.down * groundCheckDistance, Color.blue);
        
        return centerHit || leftHit || rightHit;
    }
    
    private void OnEnable()
    {
        var actionMap = inputActions.FindActionMap(actionMapName, true);
        jumpAction = actionMap.FindAction(jumpActionName, true);
        jumpAction.Enable();
        jumpAction.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        if (jumpAction != null)
        {
            jumpAction.performed -= OnJumpPerformed;
            jumpAction.Disable();
        }
    }
    
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        //jumpPressed = true;
        jumpBufferCounter = jumpBufferTime;
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        // Input salto
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
            //jumpPressed = true;
        //}
        if (pauseManager)
            if (pauseManager.IsPaused) return;

        // Ground check
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        
        // Flip personaggio
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        /*if (horizontalInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(horizontalInput) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }*/ /*
        if (horizontalInput != 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0, horizontalInput > 0 ? 0 : 180, 0);
            transform.rotation = targetRotation;
        }*/
        /*if (horizontalInput != 0)
        {
            float baseY = 90f;
            float flipY = baseY + (horizontalInput > 0 ? 0f : 180f);
            transform.rotation = Quaternion.Euler(0f, flipY, 0f);
        }
        */

        // Salva l'ultima direzione non-zero
        if (Mathf.Abs(horizontalInput) > 0.1f)
            lastNonZeroHorizontalInput = horizontalInput;

        // Applica il flip in base all'ultima direzione
        float baseY = 90f;
        float flipY = baseY + (lastNonZeroHorizontalInput > 0 ? 0f : 180f);
        transform.rotation = Quaternion.Euler(0f, flipY, 0f);
        animator.SetBool("isJumping", !isGrounded);
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float currentSpeed = isGrounded ? moveSpeed : airMoveSpeed;

        /*if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpPressed = false;
        }*/
        
        /*
        // Countdown del buffer
        if (jumpBufferCounter > 0f)
            jumpBufferCounter -= Time.fixedDeltaTime;

        // Salto se sei a terra e il buffer è attivo
        if (jumpBufferCounter > 0f && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpBufferCounter = 0f; // reset dopo il salto
        }*/
        
        // Countdown del buffer
        if (jumpBufferCounter > 0f)
            jumpBufferCounter -= Time.fixedDeltaTime;

        // Chek if jump
        bool canJump = coyoteTimer > 0f || jumpsRemaining > 0;

        if (jumpBufferCounter > 0f && canJump)
        {
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            float appliedJumpForce = isGrounded ? jumpForce : jumpForce * airJumpForceMultiplier;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, appliedJumpForce, rb.linearVelocity.z);
            animator.SetTrigger("Jump");
            jumpBufferCounter = 0f;

            if (isGrounded)
            {
                // keep in air
                jumpsRemaining = (int)jumpMode - 1; // es: DoubleJump = 2 → 1 jump in air
            }
            else
            {
                //another jump in the air
                jumpsRemaining--;
            }
        }
        
        // horizontal movement
       /* Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * moveSpeed;
        rb.linearVelocity = velocity;*/
        Vector3 velocity = rb.linearVelocity;
        //velocity.x = horizontalInput * currentSpeed;
        //flip deceleration
        bool isChangingDirection = Mathf.Sign(horizontalInput) != Mathf.Sign(previousHorizontalInput) && horizontalInput != 0f && previousHorizontalInput != 0f;

        if (isChangingDirection)
        {
            // Applica rallentamento durante cambio direzione
            // tra 0 e 1, più basso = più lento
            velocity.x = Mathf.Lerp(rb.linearVelocity.x, horizontalInput * currentSpeed, decelerationFactor);
        }
        else
        {
            // Movimento normale
            velocity.x = horizontalInput * currentSpeed;
        }
        previousHorizontalInput = horizontalInput;

        rb.linearVelocity = velocity;

        //isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = IsGrounded();
        if (isGrounded)
        {
            coyoteTimer = coyoteTime; // reset on gorund
            jumpsRemaining = (int)jumpMode - 1;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
        
        if (isGrounded)
        {
            jumpsRemaining = (int)jumpMode - 1; // reset on extra jump
        }
       // Debug.Log($"isGrounded: {isGrounded}, jumpPressed: {jumpPressed}");

        /*// Salto
        if (jumpPressed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            //rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

            jumpPressed = false;
        }*/

        // Gravità 
        /*if (!isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }*/
        float extraGravity = 0f;

        if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                // up → apply less gravity
                extraGravity = Physics.gravity.y * (ascentGravityMultiplier - 1);
            }
            else if (rb.linearVelocity.y < 0)
            {
                // down → apply more gravity
                extraGravity = Physics.gravity.y * (gravityMultiplier - 1);
            }

            rb.linearVelocity += Vector3.up * extraGravity * Time.fixedDeltaTime;
        }
        
        
        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);

        // Aggiorna blend tree Animator
        float currentSpeedA = animator.GetFloat("Speed");
        float smoothSpeed = Mathf.Lerp(currentSpeedA, horizontalSpeed, animationSpeedSmooth * Time.fixedDeltaTime);
        animator.SetFloat("Speed", Mathf.Clamp01(smoothSpeed));
        
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
