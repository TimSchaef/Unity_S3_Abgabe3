using UnityEngine;

public class WeaponFollowFacing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement movement;      // sitzt am Player (Parent)
    [SerializeField] private SpriteRenderer weaponRenderer; // SpriteRenderer am Weapons-Objekt

    [Header("Local Positions")]
    [SerializeField] private Vector2 rightLocalPos = new Vector2(0.52f, 0.03f);
    [SerializeField] private Vector2 leftLocalPos  = new Vector2(-0.52f, 0.03f);

    [Header("Optional: Flip sprite for correct look")]
    [SerializeField] private bool flipSprite = true;

    private void Awake()
    {
        if (movement == null) movement = GetComponentInParent<PlayerMovement>();
        if (weaponRenderer == null) weaponRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (movement == null) return;

        bool facingLeft = movement.FacingX < 0f;

        // 1) Waffe auf die andere Seite setzen (Position spiegeln)
        transform.localPosition = facingLeft ? (Vector3)leftLocalPos : (Vector3)rightLocalPos;

        // 2) Optional: Sprite flippen, damit die Waffe nicht "rückwärts" aussieht
        if (flipSprite && weaponRenderer != null)
            weaponRenderer.flipX = facingLeft;
    }
}

