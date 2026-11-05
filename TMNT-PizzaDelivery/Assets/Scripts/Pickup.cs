using UnityEngine;

public class Pickup : MonoBehaviour
{
    public AudioClip pickupSound;
    private bool didPickupAlready = false;
    public LevelManager LM;

    private void Start()
    {
        if (LM == null)
        {
            LM = GameObject.Find("Canvas").GetComponent<LevelManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !didPickupAlready)
        {
            didPickupAlready = true;

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Update coin stats on the LevelManager
            if (LM != null)
            {
                LM.numberOfCoins++;
                LM.coinCounterText.SetText("Moneys: " + LM.numberOfCoins);
            }

            Destroy(gameObject);
        }
    }
}