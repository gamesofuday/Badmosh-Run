using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;    // Speed of the player's forward movement
    public float jumpForce = 10f;  // Force of the jump
    public float crouchScale = 0.5f; // Scale factor for crouching
    private Rigidbody2D rb;
    private bool isGrounded = true; // Check if player is on the ground
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale; // Save original scale for crouching
    }

    void Update()
    {
        // Move the player forward continuously
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

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
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply upward force
        isGrounded = false; // Player is no longer grounded
    }

    private void Crouch()
    {
        // Reduce player's height to simulate crouching
        transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchScale, originalScale.z);
    }

    private void StandUp()
    {
        // Reset player's height
        transform.localScale = originalScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player lands on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

