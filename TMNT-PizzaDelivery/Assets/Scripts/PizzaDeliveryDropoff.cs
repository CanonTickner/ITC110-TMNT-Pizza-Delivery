using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PizzaDeliveryDropoff : MonoBehaviour
{
    [Header("Optional Override Audio (Leave empty if set on ScoreManager)")]
    [SerializeField] private AudioClip deliverySound;
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 1f;

    [Header("Visual Feedback (Optional)")]
    [Tooltip("Optional object or checkmark that turns on after delivery.")]
    [SerializeField] private GameObject deliveredVFX;

    private bool alreadyDelivered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyDelivered) return;

        if (other.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null && ScoreManager.Instance.DeliverPizza())
            {
                alreadyDelivered = true;

                if (deliverySound != null)
                {
                    ScoreManager.Instance.PlaySFX(deliverySound, soundVolume);
                }

                if (deliveredVFX != null)
                {
                    deliveredVFX.SetActive(true);
                }
            }
        }
    }
}