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

        // Move the player forward continuously
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // Update animation states
        if (isGrounded)
        {
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsFalling", false);
        }
        else
        {
            animator.SetBool("IsRunning", false);

            // Check if the player is falling
            if (rb.velocity.y < 0)
            {
                animator.SetBool("IsFalling", true);
            }
            else
            {
                animator.SetBool("IsFalling", false);
            }
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

    // Start the game
    public void StartGame()
    {
        Debug.Log("Game Started!"); // Add this line
        isGameRunning = true;
        menuUI.SetActive(false);
        animator.SetBool("IsRunning", true);
    }

    // Show the menu
    public void ShowMenu()
    {
        // Reset the game state
        isGameRunning = false;
        Time.timeScale = 1f; // Resume the game if paused
        rb.velocity = Vector2.zero; // Stop player movement
        transform.position = new Vector3(0, transform.position.y, 0); // Reset player position
        menuUI.SetActive(true);  // Enable menu UI
        gameOverUI.SetActive(false); // Hide game over UI
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsGrounded", true); // Set idle animation
    }

    private void GameOver()
    {
        isGameRunning = false;
        animator.SetBool("IsRunning", false);
        Time.timeScale = 0f; // Pause the game
        gameOverUI?.SetActive(true); // Show game over UI
    }

}
