using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CoinPickup : MonoBehaviour
{
    [Header("Optional Override Audio (Leave empty if set on ScoreManager)")]
    [SerializeField] private AudioClip pickupSound;
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 0.5f;

    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.CollectCoin();

                if (pickupSound != null)
                {
                    ScoreManager.Instance.PlaySFX(pickupSound, soundVolume);
                }
            }

            gameObject.SetActive(false);
        }
    }
}