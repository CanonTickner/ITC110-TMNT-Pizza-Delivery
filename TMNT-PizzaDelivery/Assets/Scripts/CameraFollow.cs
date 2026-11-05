using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The player GameObject to follow (e.g., KnightPlayer).")]
    [SerializeField] private Transform target;

    [Header("Offsets & Alignment")]
    [Tooltip("Offset relative to the player. Adjust X to push the player left on the screen.")]
    [SerializeField] private Vector3 offset = new Vector3(3f, 1.5f, -10f);

    [Header("Axis Locking")]
    [Tooltip("When enabled, the camera's vertical position is locked so it does not bounce when jumping.")]
    [SerializeField] private bool lockYAxis = true;

    [Tooltip("Fixed Y world position used when lockYAxis is true. Defaults to starting height if left at 0.")]
    [SerializeField] private float fixedYPosition = 0f;

    [Header("Damping / Smoothing")]
    [Tooltip("Smoothing lag. Set to 0 for instant, 1:1 rigid tracking.")]
    [Range(0f, 0.5f)]
    [SerializeField] private float smoothTime = 0f;

    private Vector3 currentVelocity = Vector3.zero;

    private void Start()
    {
        // If no fixed Y was entered, lock onto the camera's initial scene height
        if (Mathf.Approximately(fixedYPosition, 0f))
        {
            fixedYPosition = transform.position.y;
        }

        // Auto-assign player by tag if the field was left empty in Inspector
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate horizontal target with offset
        float targetX = target.position.x + offset.x;

        // Lock Y or follow target Y
        float targetY = lockYAxis ? fixedYPosition : (target.position.y + offset.y);

        // Maintain fixed 2D camera depth (typically -10)
        float targetZ = offset.z;

        Vector3 targetPosition = new Vector3(targetX, targetY, targetZ);

        if (smoothTime > 0f)
        {
            // Smoothly interpolate to position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        }
        else
        {
            // Direct lock-step tracking (eliminates visual jitter in runners)
            transform.position = targetPosition;
        }
    }
}