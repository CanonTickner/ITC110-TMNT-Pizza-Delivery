using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private Animator animator;
    private Collider2D[] allColliders;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        allColliders = GetComponentsInChildren<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore further hits if already dying
        if (isDead) return;

        if (other.gameObject.CompareTag("Bullet"))
        {
            isDead = true;

            // Destroy the bullet
            Destroy(other.gameObject);

            // Disable all colliders so it cannot hit the player or triggers while dying
            foreach (Collider2D col in allColliders)
            {
                col.enabled = false;
            }

            // Stop physics/movement if Rigidbody exists
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            // Disable movement scripts (PitMonster, PitMonsterUpDown, etc.)
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour s in scripts)
            {
                if (s != this)
                {
                    s.enabled = false;
                }
            }

            // Play death animation if animator exists
            if (animator != null)
            {
                animator.SetTrigger("gotKilled");
                Destroy(gameObject, 0.5f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}