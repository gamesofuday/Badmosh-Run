using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // Speed of the player's forward movement
    public float jumpForce = 10f;     // Force of the jump
    public float crouchScale = 0.5f;  // Scale factor for crouching
    private Rigidbody2D rb;
    public bool isGrounded = false;   // Check if player is on the ground
    private Vector3 originalScale;
    private Animator animator;

    public Transform groundCheck;     // Transform for ground check position
    public float groundCheckRadius = 0.2f; // Radius for ground check
    public LayerMask groundLayer;     // Layer mask for ground

    // UI Elements
    public GameObject menuUI;         // Menu UI (Start button and title)
    public GameObject gameOverUI;     // Game Over UI (optional)
    public Text gameOverText;         // Game Over Text (optional)

    private bool isGameRunning = false; // Tracks if the game has started
    private bool isFalling = false;     // Tracks if the player is falling

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale; // Save original scale for crouching
        animator = GetComponent<Animator>();

        // Show menu and set up idle animation
        ShowMenu();
    }

    void Update()
    {
        if (!isGameRunning) return; // Skip game logic if the game is not running

        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Update falling state
        if (!isGrounded && rb.velocity.y < 0) // Player is in the air and falling
        {
            isFalling = true;
            animator.SetBool("IsFalling", true); // Play falling animation
        }
        else
        {
            isFalling = false;
            animator.SetBool("IsFalling", false); // Stop falling animation
        }

        // Move the player forward continuously
        if (transform.localScale == originalScale && isGrounded) // Don't move if crouched or airborne
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
        animator.SetTrigger("Jump"); // Play jump animation
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player hits an obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    // Show the menu
    private void ShowMenu()
    {
        menuUI.SetActive(true);  // Enable menu UI
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsGrounded", true); // Set idle animation
    }

    // Start the game
    public void StartGame()
    {
        Debug.Log("Game Started!"); // Add this line
        isGameRunning = true;
        menuUI.SetActive(false);
        animator.SetBool("IsRunning", true);
    }

    // Game over logic
    private void GameOver()
    {
        isGameRunning = false;
        animator.SetBool("IsRunning", false);
        Time.timeScale = 0f; // Pause the game
        gameOverUI?.SetActive(true); // Optional: Show game over UI
    }
}
