using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class TurtleController : MonoBehaviour
{
    // movement variables
    public bool controlsEnabled = true;
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float fallGravityMultiplier = 2f;
    public float normalGravityScale = 1f;
    public float iceAcceleration = 5f;
    public float airAcceleration = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask whatIsGround;
    public LayerMask iceLayer;
    public GameManager gameManager;
    public SpriteRenderer spriteRenderer;

    //other
    public float lifetime;
    public Slider healthBar;
    public GameObject shellPrefab;
    public GameObject camera;

    public Rigidbody2D rb;
    private bool isGrounded;
    public float moveInput;
    private CapsuleCollider2D capsuleCollider;
    private bool dead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        airAcceleration = moveSpeed;
        iceAcceleration = 5 * moveSpeed;

        if (controlsEnabled && !dead)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            if (moveInput > 0)
                spriteRenderer.flipX = false; // Facing right
            else if (moveInput < 0)
                spriteRenderer.flipX = true; // Facing left
            if (Input.GetButtonDown("Jump") && isGrounded)
                Jump();

            lifetime -= Time.deltaTime;
            if ((lifetime <= 0 || Input.GetKeyDown(KeyCode.Q)) && !dead)
            {
                dead = true;
                StartCoroutine(turtleDeath());
            }
        }
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

        if (!onIce)
        {
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

        if (controlsEnabled)
        {
            rb.linearVelocity = new Vector2(currentVelX, rb.linearVelocity.y);
        }
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

    public IEnumerator turtleDeath()
    {
        Debug.Log("TurtleDeath called");
        bool once = true;
        // Freeze all movement
        rb.constraints = RigidbodyConstraints2D.FreezeAll; 
        capsuleCollider.enabled = false; // Disable collider to prevent further interactions
        // Play death animation
        yield return new WaitForSeconds(2f);
        GameObject shell = Instantiate(shellPrefab, transform.position, Quaternion.identity);
        if (once)
        {
            once = false;
            gameManager.initiateNextTurtleLife();
        }
        yield return new WaitForSeconds(2f);
        Destroy(camera); // Destroy the camera object
        Destroy(gameObject); // Destroy the turtle object
        // Disable camera and pan back to start
        // Get rid of turtle and replace it with shell object
    }
}


