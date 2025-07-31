using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float groundMoveSpeed = 10f;
    public float iceAcceleration = 20f;
    public float maxIceSpeed = 10f;

    [Header("Jump Settings")]
    public float jumpForce = 12f;

    [Header("Gravity Settings")]
    public float gravity = -20f;
    public float fallMultiplier = 2f;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Vector2 velocity;
    private bool isGrounded;
    private bool isOnIce;

    private float horizontalInput;

    void Update()
    {
        // Read input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpForce;
        }
    }

    void FixedUpdate()
    {
        ApplyGravity();
        Move();
    }

    void Move()
    {
        // Check ground and ice state
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isOnIce = isGrounded && CheckIce();

        // Horizontal Movement
        if (isGrounded && !isOnIce)
        {
            // Instant movement on ground
            velocity.x = horizontalInput * groundMoveSpeed;
        }
        else
        {
            // Acceleration movement on ice or in air
            float targetSpeed = horizontalInput * maxIceSpeed;
            float acceleration = iceAcceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, targetSpeed, acceleration);
        }
    }

    void ApplyGravity()
    {
        bool falling = velocity.y < 0 && !isGrounded;
        float appliedGravity = falling ? gravity * fallMultiplier : gravity;
        velocity.y += appliedGravity * Time.fixedDeltaTime;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        // Move the character
        transform.Translate(velocity * Time.fixedDeltaTime);
    }

    bool CheckIce()
    {
        // Replace this with your own method (tag check, layer check, material, etc.)
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);
        return hit && hit.collider.CompareTag("Ice");
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
