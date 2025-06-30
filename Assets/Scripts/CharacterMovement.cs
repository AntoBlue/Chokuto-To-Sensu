using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float dieFromFallingTime = 1f;

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
    private bool hasJumped = false;
    
    //double jump less strong
    [SerializeField] private float airJumpForceMultiplier = 0.8f;
    
    //animator
    [SerializeField] private Animator animator;
    private int totalBodyLayerIndex;

    public bool hasKey;
    
    //flip & deceleration
    private float previousHorizontalInput = 0f;
    [SerializeField] private float decelerationFactor = 0.85f;
    private float lastNonZeroHorizontalInput = 1f; // default facing right
    public bool IsFacingRight { get; private set; }
    
    //Raycast isGrounded
    [Header("Advanced Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float raycastOffset = 0.3f;

    [SerializeField] private PauseManager pauseManager;
    
    // Falling timer ( die from falling timer ) 
    private float fallingTimer;
    private bool isFalling = false;

    private bool HasAlreadyJumped()
    {
        return jumpsRemaining < (int)jumpMode - 1;
    }
    
    public void setDoubleJump()
    {
        jumpMode = JumpMode.DoubleJump;
    }
    
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
            //Debug.Log($"isGrounded");
        }
        
        
        Debug.DrawRay(originLeft, Vector3.down * groundCheckDistance, leftHit ? Color.green : Color.yellow);
        Debug.DrawRay(originRight, Vector3.down * groundCheckDistance, rightHit ? Color.blue : Color.yellow);
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

    private void Start()
    {
        totalBodyLayerIndex = animator.GetLayerIndex("TotalBody");
        animator.SetLayerWeight(totalBodyLayerIndex, 1f);
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
        {
            lastNonZeroHorizontalInput = horizontalInput;
            IsFacingRight = horizontalInput > 0;
        }
            

        // Applica il flip in base all'ultima direzione
        float baseY = 90f;
        float flipY = baseY + (lastNonZeroHorizontalInput > 0 ? 0f : 180f);
        transform.rotation = Quaternion.Euler(0f, flipY, 0f);
        //animator.SetBool("isJumping", !isGrounded);
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float currentSpeed = isGrounded ? moveSpeed : airMoveSpeed;

        float normalizedX = rb.linearVelocity.x / moveSpeed;
        float normalizedY = rb.linearVelocity.y / jumpForce;
        
        bool wasGrounded = isGrounded;

        // Blend tree parameters
        animator.SetFloat("VelocityX", normalizedX);
        animator.SetFloat("VelocityY", normalizedY);
        
        
        if (Mathf.Abs(normalizedX) > 0.0001f || Mathf.Abs(normalizedY) > 0.0001f )
            animator.SetLayerWeight(totalBodyLayerIndex, 0f);
        else
            animator.SetLayerWeight(totalBodyLayerIndex, 1f);
        
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
        //bool canJump = coyoteTimer > 0f || jumpsRemaining > 0;
        //bool canJump = (isGrounded || coyoteTimer > 0f) && (int)jumpMode > 0 || jumpsRemaining > 0;
        bool canJump = isGrounded || jumpsRemaining > 0 || (coyoteTimer > 0f && (int)jumpMode > 0 && !hasJumped);

        if (jumpBufferCounter > 0f && canJump)
        {
            float appliedJumpForce = isGrounded ? jumpForce : jumpForce * airJumpForceMultiplier;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, appliedJumpForce, rb.linearVelocity.z);
            jumpBufferCounter = 0f;

            if (!isGrounded)
            {
                jumpsRemaining--;
                jumpsRemaining = Mathf.Max(0, jumpsRemaining);
                
            }
            else
            {
                hasJumped = true;
            }

            if ((coyoteTimer > 0f && (int)jumpMode > 1 && !hasJumped))
            {
                jumpsRemaining++;//fast fix
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
            // slowed movement at flip
            velocity.x = Mathf.Lerp(rb.linearVelocity.x, horizontalInput * currentSpeed, decelerationFactor);
        }
        else
        {
            // regular movement
            velocity.x = horizontalInput * currentSpeed;
        }
        previousHorizontalInput = horizontalInput;

        rb.linearVelocity = velocity;

        //isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        
        isGrounded = IsGrounded();

        if (isGrounded && !wasGrounded)
        {
            coyoteTimer = coyoteTime;
            jumpsRemaining = (int)jumpMode - 1;
            hasJumped = false;
        }
        else if (!isGrounded)
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
        float extraGravity = 0f;

        if (!isGrounded)
        {
            // player will die for falling 4 more than N seconds ( check dieFromFallingTime ) -----------------------------------------------------------------------------------------
            if (rb.linearVelocity.y < -0.1f)
            {
                if (!isFalling)
                {
                    isFalling = true;
                    fallingTimer = 0f;
                }
                
                fallingTimer += Time.fixedDeltaTime;
                
                if (fallingTimer > dieFromFallingTime)
                {
                    //Destroy(gameObject);
                    SceneManager.LoadScene("GameOverScene");

                    //Debug.Log("Gameobject destroyed");
                }
            }
            else
            {
                isFalling = false;
                fallingTimer = 0f;
            }
            
            
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
      
        
        //float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        
        // Aggiorna blend tree Animator
        //float currentSpeedA = animator.GetFloat("Speed");
        //float smoothSpeed = Mathf.Lerp(currentSpeedA, horizontalSpeed, animationSpeedSmooth * Time.fixedDeltaTime);
        //animator.SetFloat("Speed", Mathf.Clamp01(smoothSpeed));
        
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
