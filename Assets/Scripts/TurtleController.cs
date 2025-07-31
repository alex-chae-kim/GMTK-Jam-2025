using UnityEngine;

public class TurtleController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Maximum horizontal movement speed.")]
    public float moveSpeed = 5f;

    [Tooltip("Force applied when jumping.")]
    public float jumpForce = 12f;

    [Header("Gravity Settings")]
    [Tooltip("Gravity scale applied when falling.")]
    public float fallGravityMultiplier = 2f;

    [Tooltip("Gravity scale applied when rising or grounded.")]
    public float normalGravityScale = 1f;

    [Header("Acceleration Settings (ice & air)")]
    [Tooltip("Rate at which character accelerates/decelerates on ice (units/sec�).")]
    public float iceAcceleration = 5f;

    [Tooltip("Rate at which character accelerates/decelerates in air (units/sec�).")]
    public float airAcceleration = 10f;

    [Header("Ground Check Settings")]
    [Tooltip("Transform used as the origin for ground checking.")]
    public Transform groundCheck;

    [Tooltip("Radius of the circle used to check for ground.")]
    public float groundCheckRadius = 0.1f;

    [Tooltip("Layers considered as ground.")]
    public LayerMask whatIsGround;

    [Tooltip("Layer mask for ice surfaces.")]
    public LayerMask iceLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        airAcceleration = moveSpeed;
        iceAcceleration = 5 * moveSpeed;

        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();
    }

    void FixedUpdate()
    {
        // Adjust gravity scale: double when falling, normal when rising or grounded
        if (rb.linearVelocity.y < 0)
            rb.gravityScale = fallGravityMultiplier;
        else
            rb.gravityScale = normalGravityScale;

        // Check ground & surface
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround) || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, iceLayer);
        bool onIce = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, iceLayer);

        float targetVelX = moveInput * moveSpeed;
        float currentVelX = rb.linearVelocity.x;

        if (isGrounded && !onIce)
        {
            // Instant speed change on normal ground
            currentVelX = targetVelX;
        }
        else
        {
            // Gradual acceleration/deceleration on ice or in air
            float accel = isGrounded ? iceAcceleration : airAcceleration;
            if (moveInput == 0)
            {
                accel = accel * 0.1f;
            }
            currentVelX = Mathf.MoveTowards(currentVelX, targetVelX, accel * Time.fixedDeltaTime);
        }

        rb.linearVelocity = new Vector2(currentVelX, rb.linearVelocity.y);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}


