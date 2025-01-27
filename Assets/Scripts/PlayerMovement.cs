using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float jumpForce = 10f;
    public float crouchScale = 0.5f;

    [Header("References")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 originalScale;
    private bool isGrounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }

        animator.SetBool("IsGrounded", isGrounded);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("Jump");
    }

    private void Crouch()
    {
        transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchScale, originalScale.z);
        animator.SetBool("IsCrouching", true);
    }

    private void StandUp()
    {
        transform.localScale = originalScale;
        animator.SetBool("IsCrouching", false);
    }

    public void StartGame()
    {
        animator.SetBool("IsRunning", true);
    }

    public void StopGame()
    {
        animator.SetBool("IsRunning", false);
    }

    public void RestartGame()
    {
        transform.position = Vector3.zero;
        isGrounded = true;
        StartGame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            FindObjectOfType<GameManager>().GameOver();
        }
    }
}
