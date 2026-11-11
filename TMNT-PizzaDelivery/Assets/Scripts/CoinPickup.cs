using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CoinPickup : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("Audio clip played when the turtle collects this coin.")]
    [SerializeField] private AudioClip pickupSound;
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 0.8f;

    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            // Play the audio clip at the camera position so the 2D audio isn't muted or attenuated
            if (pickupSound != null)
            {
                Vector3 soundPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
                AudioSource.PlayClipAtPoint(pickupSound, soundPosition, soundVolume);
            }

            // Add score and disable the coin
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(1);
            }

            gameObject.SetActive(false);
        }
    }
}