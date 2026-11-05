using UnityEngine;
using UnityEngine.SceneManagement;

public class EpicFail : MonoBehaviour
{
    public LevelManager LM;

    void Start()
    {
        if (LM == null)
        {
            LM = GameObject.Find("Canvas").GetComponent<LevelManager>();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("there went Triangle Boy!");

            if (LM != null)
            {
                // 1. Subtract a life
                LM.numberOfLives--;

                // 2. Update the UI text
                LM.lifeCounterText.SetText("Lives: " + LM.numberOfLives);

                // 3. Check if out of lives
                if (LM.numberOfLives <= 0)
                {
                    // Full reset when all lives are lost
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                else
                {
                    // Respawn player at the checkpoint / spawn point
                    other.transform.position = LM.spawnPoint;

                    // Reset physics velocity so the player doesn't carry momentum
                    Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector2.zero;
                    }
                }
            }
        }
    }
}