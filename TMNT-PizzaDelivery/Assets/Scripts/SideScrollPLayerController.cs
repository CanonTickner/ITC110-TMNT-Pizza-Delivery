using UnityEngine;
using UnityEngine.InputSystem;

public class KnightPlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float jumpForce = 50.0f;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public bool isGrounded = false;
    public InputActionReference playerMove;
    public InputActionReference playerJump;
    public InputActionReference playerAttack;
    public AudioClip jumpSound;
    public GameObject bullet;
    public GameObject firePoint;
    public bool fireForward = true;
    public float bulletForce = 1500.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // get input from Input System
        Vector2 directionP = playerMove.action.ReadValue<Vector2>();
        directionP.y = 0;
        transform.Translate(directionP * moveSpeed * Time.deltaTime);

        // animate!
        if (directionP.x > 0)
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = false;
            fireForward = true;
        }
        else if (directionP.x < 0)
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = true;
            fireForward = false;
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void OnEnable()
    {
        playerJump.action.started += JumpPlayer;
        playerAttack.action.started += SwordAttack;
    }

    private void OnDisable()
    {
        playerJump.action.started -= JumpPlayer;
        playerAttack.action.started -= SwordAttack;
    }

    void JumpPlayer(InputAction.CallbackContext context)
    {
        if (isGrounded == true)
        {
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            rb.AddForce(transform.up * jumpForce);
            animator.SetBool("isJumping", true);
        }
    }

    void SwordAttack(InputAction.CallbackContext context)
    {
        // heres the aggression!
        animator.SetTrigger("doAttack");
        FireBullet();
    }

    void FireBullet()
    {
        // Bullet instantiate at the position of GameObject
        GameObject newBullet = Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation) as GameObject;

        // get Rigidbody2D component of instantiated Bullet
        Rigidbody2D tempRigidBody = newBullet.GetComponent<Rigidbody2D>();

        // push the Bullet forward by amount bulletForce
        if (fireForward)
        {
            // fireForward is fire to the right
            tempRigidBody.AddForce(transform.right * bulletForce);
        }
        else
        {
            // fire left, a.k.a. "negative right"
            tempRigidBody.AddForce(-transform.right * bulletForce);
        }

        // basic Clean Up, set Bullets to self destruct after 2 seconds
        Destroy(newBullet, 2.0f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }
}