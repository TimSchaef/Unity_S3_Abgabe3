using UnityEngine;

public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] private Transform player; // optional, wenn du es automatisch finden willst
    [SerializeField] private PlayerMovement movement;

    private void Awake()
    {
        if (weaponRenderer == null) weaponRenderer = GetComponent<SpriteRenderer>();

        if (movement == null)
        {
            // PlayerMovement sitzt am Parent "Player"
            movement = GetComponentInParent<PlayerMovement>();
        }
    }

    private void LateUpdate()
    {
        if (movement == null || weaponRenderer == null) return;

        // FacingX: +1 rechts, -1 links
        weaponRenderer.flipX = movement.FacingX < 0f;
    }
}

