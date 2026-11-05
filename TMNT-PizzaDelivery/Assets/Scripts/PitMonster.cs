using UnityEngine;

public class PitMonster : MonoBehaviour
{
    public bool goLeft = true;
    
    public float moveSpeed = 2.0f;
    public int direction = 1;
    public Vector2 startingScale;

    private Animator animator;
    private Collider2D[] allColliders;
    private bool isDying = false;

    void Start()
    {
        startingScale = transform.localScale;
        animator = GetComponent<Animator>();
        allColliders = GetComponentsInChildren<Collider2D>();
    }

    void Update()
    {
        // Don't move if currently playing the death animation
        if (isDying) return;

        // Going left along negative direction
        if (goLeft == true)
        {
            direction = -1;
            transform.localScale = startingScale;
        }
        else
        {
            direction = 1;
            transform.localScale = new Vector2(-startingScale.x, startingScale.y);
        }

        transform.Translate(new Vector2(direction, 0) * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDying) return;

        if (collision.CompareTag("Bullet"))
        {
            // Destroy the bullet
            Destroy(collision.gameObject);

            // If this enemy has an Animator with a death animation (like the Slime)
            if (animator != null)
            {
                isDying = true;

                // Disable colliders so it can't harm the player while dying
                foreach (Collider2D col in allColliders)
                {
                    col.enabled = false;
                }

                // Play death animation
                animator.SetTrigger("gotKilled");

                // Wait 0.5s for animation to finish before deleting
                Destroy(gameObject, 0.5f);
            }
            else
            {
                // Evil eyes (no Animator) disappear immediately
                Destroy(gameObject);
            }
        }
        else
        {
            // Turn around when hitting patrol edges/walls
            goLeft = !goLeft;
        }
    }
}