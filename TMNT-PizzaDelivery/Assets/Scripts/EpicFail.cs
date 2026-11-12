using UnityEngine;
using UnityEngine.SceneManagement;

public class EpicFail : MonoBehaviour
{
    private bool isResetting = false;

    // Triggers if the obstacle's Collider 2D has "Is Trigger" CHECKED
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FullReset();
        }
    }

    // Triggers if the obstacle's Collider 2D has "Is Trigger" UNCHECKED (solid obstacle)
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FullReset();
        }
    }

    private void FullReset()
    {
        if (isResetting) return;
        isResetting = true;

        Debug.Log("Game Over - Full Reset!");

        // 1. Stop the timer and reset the score counter to 0
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.StopScoring();
            ScoreManager.Instance.ResetScore();
        }

        // 2. Reload the entire level so all pickups and positions reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}