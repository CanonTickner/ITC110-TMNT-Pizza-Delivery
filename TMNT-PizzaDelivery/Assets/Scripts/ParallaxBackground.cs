using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackground : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("0 = moves locked with camera (very far away), 1 = moves at normal ground speed. Try 0.3 to 0.5.")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxEffect = 0.4f;

    private float startPosX;
    private float spriteWidth;
    private Transform camTransform;

    private void Start()
    {
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }

        startPosX = transform.position.x;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;

        // Automatically create seamless left and right copies so the city loops infinitely
        CreateNeighborClone(-spriteWidth, sr);
        CreateNeighborClone(spriteWidth, sr);
    }

    private void LateUpdate()
    {
        if (camTransform == null) return;

        // How far the camera has moved relative to the background
        float relativeCameraDist = camTransform.position.x * (1f - parallaxEffect);

        // How far the background should shift in world space
        float parallaxOffset = camTransform.position.x * parallaxEffect;

        transform.position = new Vector3(startPosX + parallaxOffset, transform.position.y, transform.position.z);

        // Seamlessly wrap the background position when the camera passes one full image width
        if (relativeCameraDist > startPosX + spriteWidth)
        {
            startPosX += spriteWidth;
        }
        else if (relativeCameraDist < startPosX - spriteWidth)
        {
            startPosX -= spriteWidth;
        }
    }

    private void CreateNeighborClone(float offsetX, SpriteRenderer originalSR)
    {
        GameObject clone = new GameObject(gameObject.name + "_Tile");
        clone.transform.SetParent(transform);
        clone.transform.localPosition = new Vector3(offsetX / transform.localScale.x, 0f, 0f);
        clone.transform.localScale = Vector3.one;

        SpriteRenderer cloneSR = clone.AddComponent<SpriteRenderer>();
        cloneSR.sprite = originalSR.sprite;
        cloneSR.sortingLayerID = originalSR.sortingLayerID;
        cloneSR.sortingOrder = originalSR.sortingOrder;
        cloneSR.color = originalSR.color;
    }
}