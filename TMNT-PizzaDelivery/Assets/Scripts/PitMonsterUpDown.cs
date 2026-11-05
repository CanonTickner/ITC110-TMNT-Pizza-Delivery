using UnityEngine;

public class PitMonsterUpDown : MonoBehaviour
{
    public bool goUp = true;
    
    public float moveSpeed = 2.0f;
    public int direction = 1;
    public Vector2 startingScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Moving along the Y axis
        if (goUp)
        {
            direction = 1;
            transform.localScale = startingScale;
        }
        else
        {
            direction = -1;
            transform.localScale = new Vector2(startingScale.x, -startingScale.y);
        }

        // Translate vertically (X = 0, Y = direction)
        transform.Translate(new Vector2(0, direction) * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If hit by a bullet, destroy both the bullet and this enemy
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else
        {
            // Reverse patrol direction when hitting bounds/walls
            goUp = !goUp;
        }
    }
}