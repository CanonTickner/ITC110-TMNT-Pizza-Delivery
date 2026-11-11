using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PizzaDeliveryDropoff : MonoBehaviour
{
    [Tooltip("Visual marker/checkmark that activates upon successful drop-off.")]
    [SerializeField] private GameObject deliveredVFX;

    private bool alreadyDelivered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyDelivered) return;

        if (other.CompareTag("Player"))
        {
            if (ScoreManager.Instance.DeliverPizza())
            {
                alreadyDelivered = true;

                if (deliveredVFX != null)
                {
                    deliveredVFX.SetActive(true);
                }
            }
        }
    }
}