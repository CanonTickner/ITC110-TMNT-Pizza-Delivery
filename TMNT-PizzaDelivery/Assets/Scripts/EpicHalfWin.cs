using UnityEngine;

public class EpicHalfWin : MonoBehaviour
{
    public LevelManager LM;
    private bool hasHitCheckpoint = false;
    public Color endColor = Color.aquamarine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LM = GameObject.Find("Canvas").GetComponent<LevelManager>();
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasHitCheckpoint)
        {
            hasHitCheckpoint = true;
            // tell LM about it
            LM.spawnPoint = transform.position;
            GetComponent<SpriteRenderer>().color = endColor;

        }
    }
}