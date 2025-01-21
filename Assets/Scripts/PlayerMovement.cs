using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;       // Speed of the player's forward movement
    public float jumpForce = 10f;     // Force of the jump
    public float crouchScale = 0.5f;  // Scale factor for crouching
    private Rigidbody2D rb;
    private bool isGrounded = true;   // Check if player is on the ground
    private Vector3 originalScale;
    private Animator animator;

    public Transform groundCheck;     // Transform for ground check position
    public float groundCheckRadius = 0.2f; // Radius for ground check
    public LayerMask groundLayer;     // Layer mask for ground

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale; // Save original scale for crouching
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Move the player forward continuously
        if (transform.localScale == originalScale) // Don't move if crouched
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        // Jump Input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Crouch Input
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }

        // Update grounded animation
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply upward force
        isGrounded = false; // Player is no longer grounded
        animator.SetTrigger("Jump");
    }

    private void Crouch()
    {
        // Reduce player's height to simulate crouching
        transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchScale, originalScale.z);
        animator.SetBool("IsCrouching", true);
    }

    private void StandUp()
    {
        // Reset player's height
        transform.localScale = originalScale;
        animator.SetBool("IsCrouching", false);
    }
}
