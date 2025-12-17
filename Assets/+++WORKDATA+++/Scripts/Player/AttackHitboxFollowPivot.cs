using UnityEngine;

public class AttackHitboxFollowPivot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turnPivot;
    [SerializeField] private PlayerMovement movement;

    [Header("Offsets")]
    [SerializeField] private float forwardOffset = 0.6f;
    [SerializeField] private float verticalOffset = 0f;

    private void Awake()
    {
        if (movement == null)
            movement = GetComponentInParent<PlayerMovement>();

        if (turnPivot == null)
            Debug.LogError("AttackHitboxFollowPivot: turnPivot not assigned.");
    }

    private void LateUpdate()
    {
        if (movement == null || turnPivot == null) return;

        // +1 = rechts, -1 = links
        float facing = Mathf.Sign(movement.FacingX);

        // Position relativ zum TurnPivot
        Vector2 offset =
            (Vector2)turnPivot.right * forwardOffset * facing +
            (Vector2)turnPivot.up * verticalOffset;

        transform.position = (Vector2)turnPivot.position + offset;
    }
}

