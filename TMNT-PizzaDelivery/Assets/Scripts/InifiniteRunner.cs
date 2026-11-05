using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class InifiniteRunner : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [Tooltip("Constant speed moving toward the right side of the screen.")]
    [SerializeField] private float runSpeed = 8f;

    [Header("Jump Tuning")]
    [SerializeField] private float jumpForce = 15f;
    [Tooltip("Increases downward gravity while falling for a snappier drop.")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [Tooltip("Cuts the jump short if Space is released early.")]
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Responsiveness")]
    [Tooltip("Grace period allowing a jump right after rolling off a ledge.")]
    [SerializeField] private float coyoteTime = 0.15f;
    [Tooltip("Buffers a jump tap made just before landing on the ground.")]
    [SerializeField] private float jumpBufferTime = 0.15f;

    [Header("Tilemap Ground Detection")]
    [Tooltip("Distance below the collider bounds to probe for tilemaps.")]
    [SerializeField] private float castDistance = 0.15f;
    [Tooltip("Leave as Everything or set to your Tilemap Ground layer.")]
    [SerializeField] private LayerMask groundLayer = ~0;

    [Header("Visuals")]
    [SerializeField] private Animator animator;

    [Header("Audio")]
    [Tooltip("Audio clip played every time the turtle jumps.")]
    [SerializeField] private AudioClip jumpSound;
    [Range(0f, 1f)]
    [SerializeField] private float jumpSoundVolume = 0.8f;

    private Rigidbody2D rb;
    private Collider2D col;
    private AudioSource audioSource;
    private bool isGrounded;
    private bool isDead;

    private float coyoteCounter;
    private float jumpBufferCounter;

    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpTriggerHash = Animator.StringToHash("Jump");
    private static readonly int DieTriggerHash = Animator.StringToHash("Die");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Ensure an AudioSource component is available
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;

        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Update()
    {
        if (isDead) return;

        CheckGrounded();

        // Coyote Time tracking
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // New Input System jump detection
        bool jumpPressedThisFrame = false;
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpPressedThisFrame = true;
        }
        else if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            jumpPressedThisFrame = true;
        }

        if (jumpPressedThisFrame)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Jump execution
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            ExecuteJump();
        }

        if (animator != null)
        {
            animator.SetBool(IsGroundedHash, isGrounded);
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        float currentYVel = rb.linearVelocity.y;

        bool jumpHeld = (Keyboard.current != null && Keyboard.current.spaceKey.isPressed) ||
                        (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed);

        // Variable jump height & fast falling
        if (currentYVel < 0f)
        {
            currentYVel += Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (currentYVel > 0f && !jumpHeld)
        {
            currentYVel += Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }

        rb.linearVelocity = new Vector2(runSpeed, currentYVel);
    }

    private void CheckGrounded()
    {
        Bounds bounds = col.bounds;
        Vector2 boxSize = new Vector2(bounds.size.x * 0.85f, bounds.size.y);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            bounds.center,
            boxSize,
            0f,
            Vector2.down,
            castDistance,
            groundLayer
        );

        isGrounded = false;
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider != col && !hit.collider.isTrigger)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void ExecuteJump()
    {
        rb.linearVelocity = new Vector2(runSpeed, jumpForce);
        jumpBufferCounter = 0f;
        coyoteCounter = 0f;

        // Play jump sound
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound, jumpSoundVolume);
        }

        if (animator != null)
        {
            animator.SetTrigger(JumpTriggerHash);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Hazard"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Pizza"))
        {
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Hazard"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
        {
            animator.SetTrigger(DieTriggerHash);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<Collider2D>();
        if (col == null) return;

        Bounds bounds = col.bounds;
        Vector2 boxSize = new Vector2(bounds.size.x * 0.85f, bounds.size.y);

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube((Vector2)bounds.center + Vector2.down * castDistance, boxSize);
    }
}