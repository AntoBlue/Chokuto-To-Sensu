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
    [SerializeField] private string actionMapName = "Player"; // o il nome che hai usato
    [SerializeField] private string jumpActionName = "Jump";
    private InputAction jumpAction;
    [SerializeField] private float ascentGravityMultiplier = 1.5f;  // gravità in salita (rende il salto più secco)
      
    //Jump buffering
    [SerializeField] private float jumpBufferTime = 0.2f; // massimo tempo in cui il salto rimane "in memoria"
    private float jumpBufferCounter = 0f;
    
    //Double and triple Jump
    [SerializeField] private JumpMode jumpMode = JumpMode.SingleJump; // default: 1 salto
    private int jumpsRemaining;
    
    //Coyote time
    [SerializeField] private float coyoteTime = 0.15f; // tempo utile per saltare dopo aver lasciato il suolo
    private float coyoteTimer = 0f;
    
    //double jump less strong
    [SerializeField] private float airJumpForceMultiplier = 0.8f;
    
    
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

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        
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
        if (horizontalInput != 0)
        {
            float baseY = 90f;
            float flipY = baseY + (horizontalInput > 0 ? 0f : 180f);
            transform.rotation = Quaternion.Euler(0f, flipY, 0f);
        }
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

        // Controlla se puoi saltare (a terra o hai salti residui)
        bool canJump = coyoteTimer > 0f || jumpsRemaining > 0;

        if (jumpBufferCounter > 0f && canJump)
        {
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            float appliedJumpForce = isGrounded ? jumpForce : jumpForce * airJumpForceMultiplier;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, appliedJumpForce, rb.linearVelocity.z);
            
            jumpBufferCounter = 0f;

            if (isGrounded)
            {
                // Reset dei salti in aria
                jumpsRemaining = (int)jumpMode - 1; // es: DoubleJump = 2 → 1 salto in aria
            }
            else
            {
                // Hai usato un salto a mezz'aria
                jumpsRemaining--;
            }
        }
        
        // Movimento orizzontale
       /* Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * moveSpeed;
        rb.linearVelocity = velocity;*/
        Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * currentSpeed;
        rb.linearVelocity = velocity;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            coyoteTimer = coyoteTime; // reset ogni volta che tocchi terra
            jumpsRemaining = (int)jumpMode - 1;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
        
        if (isGrounded)
        {
            jumpsRemaining = (int)jumpMode - 1; // reset dei salti extra
        }
        Debug.Log($"isGrounded: {isGrounded}, jumpPressed: {jumpPressed}");

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
                // In salita → applica gravità leggera per rendere il salto più secco
                extraGravity = Physics.gravity.y * (ascentGravityMultiplier - 1);
            }
            else if (rb.linearVelocity.y < 0)
            {
                // In discesa → applica gravità normale potenziata
                extraGravity = Physics.gravity.y * (gravityMultiplier - 1);
            }

            rb.linearVelocity += Vector3.up * extraGravity * Time.fixedDeltaTime;
        }
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
