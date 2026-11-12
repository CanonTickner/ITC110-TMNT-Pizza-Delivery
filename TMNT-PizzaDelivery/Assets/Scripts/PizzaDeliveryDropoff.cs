using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PizzaDeliveryDropoff : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("Sound played when the pizza is successfully delivered.")]
    [SerializeField] private AudioClip deliverySound;
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 0.8f;

    [Header("Visual Feedback (Optional)")]
    [Tooltip("Optional object or checkmark that turns on after delivery.")]
    [SerializeField] private GameObject deliveredVFX;

    private bool alreadyDelivered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyDelivered) return;

        if (other.CompareTag("Player"))
        {
            // Checks if the player has a pizza, resets HasPizza to false, and adds +10 score
            if (ScoreManager.Instance != null && ScoreManager.Instance.DeliverPizza())
            {
                alreadyDelivered = true;

                if (deliverySound != null)
                {
                    Vector3 soundPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
                    AudioSource.PlayClipAtPoint(deliverySound, soundPosition, soundVolume);
                }

                if (deliveredVFX != null)
                {
                    deliveredVFX.SetActive(true);
                }
            }
        }
    }
}