using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PizzaRestaurantPickup : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("Audio clip played when picking up this pizza.")]
    [SerializeField] private AudioClip pickupSound;
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 0.8f;

    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            // Only collect if the player isn't already carrying a pizza
            if (ScoreManager.Instance != null && !ScoreManager.Instance.HasPizza)
            {
                collected = true;

                if (pickupSound != null)
                {
                    Vector3 soundPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
                    AudioSource.PlayClipAtPoint(pickupSound, soundPosition, soundVolume);
                }

                ScoreManager.Instance.PickUpPizza();
                gameObject.SetActive(false);
            }
        }
    }
}