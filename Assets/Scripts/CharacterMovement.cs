using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private float airMoveSpeed = 7f;
    public float jumpForce = 7f;
    public float gravityMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private bool jumpPressed;
        
    //input system
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player"; // o il nome che hai usato
    [SerializeField] private string jumpActionName = "Jump";
    private InputAction jumpAction;
  
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
        if (isGrounded)
        {
            jumpPressed = true;
        }
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
        }*/
        if (horizontalInput != 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0, horizontalInput > 0 ? 0 : 180, 0);
            transform.rotation = targetRotation;
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float currentSpeed = isGrounded ? moveSpeed : airMoveSpeed;

        // Movimento orizzontale
       /* Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * moveSpeed;
        rb.linearVelocity = velocity;*/
        Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * currentSpeed;
        rb.linearVelocity = velocity;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log($"isGrounded: {isGrounded}, jumpPressed: {jumpPressed}");

        // Salto
        if (jumpPressed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            //rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

            jumpPressed = false;
        }

        // Gravità 
        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
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
