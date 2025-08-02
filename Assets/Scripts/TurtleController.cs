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

    //Variables Altered by Powerups
    public int maxJumps = 1;
    public int numJumpsRemaining;
    

    //other
    public float lifetime;
    public Slider healthBar;
    public GameObject shellPrefab;
    public GameObject camera;
    public GameManager gameManager;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public Rigidbody2D rb;
    private bool isGrounded;
    private bool wasFalling;
    public float moveInput;
    private BoxCollider2D boxCollider;
    private PolygonCollider2D polygonCollider;
    private bool dead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (healthBar != null)
        {
            healthBar.maxValue = lifetime;
            healthBar.value = lifetime;
        }
        else
        {
            Debug.LogError("Health bar is not assigned!");
        }
    }

    void Update()
    {
        airAcceleration = moveSpeed;
        iceAcceleration = 5 * moveSpeed;

        if (controlsEnabled && !dead)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            if (moveInput > 0)
            {
                animator.SetBool("walking", true);
                transform.localScale = new Vector3(-1, 1, 1); // Facing right
            }
            else if (moveInput < 0)
            {
                animator.SetBool("walking", true);
                transform.localScale = new Vector3(1, 1, 1); // Facing left
            }
            else
            {
                animator.SetBool("walking", false);
            }
            if (Input.GetButtonDown("Jump") && numJumpsRemaining>0)
            {
                Jump();
            }
                
            if (isGrounded)
            {
                animator.SetBool("grounded", true);
                if (wasFalling)
                {
                    wasFalling = false;
                    animator.SetBool("falling", false);
                    animator.SetTrigger("landed");
                    
                }
                numJumpsRemaining = maxJumps;
            } 
            else
            {
                animator.SetBool("grounded", false);
            }
            lifetime -= Time.deltaTime;
            if (healthBar != null)
{
    healthBar.value = Mathf.Clamp(lifetime, 0, healthBar.maxValue);
}

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
        {
            rb.gravityScale = fallGravityMultiplier;
            wasFalling = true;
            animator.SetBool("falling", true);
        }
        else
        {
            rb.gravityScale = normalGravityScale;
            animator.SetBool("falling", false);
        }
            

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
        animator.SetTrigger("jump");
        numJumpsRemaining--;
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
        Destroy(boxCollider); // Remove box collider
        Destroy(polygonCollider); // Remove polygon collider
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

    public void ResetLifetime(float newLifetime)
    {
        lifetime = newLifetime;
        if (healthBar != null)
        {
            healthBar.maxValue = lifetime;
            healthBar.value = lifetime;
        }
        dead = false;
    }

}
