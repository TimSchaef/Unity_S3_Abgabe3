using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float hitboxActiveTime = 0.15f;

    [Header("Hitbox")]
    [Tooltip("Collider2D used as attack hitbox (must be a Trigger)")]
    [SerializeField] private Collider2D attackHitbox;

    [Header("Target Filter")]
    [SerializeField] private LayerMask enemyLayers;

    [Header("Weapon Animator (Child)")]
    [SerializeField] private Animator weaponAnimator;

    private PlayerMovement movement;

    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int MoveXHash  = Animator.StringToHash("MoveX");

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();

        if (weaponAnimator == null)
            weaponAnimator = GetComponentInChildren<Animator>();

        if (attackHitbox != null)
            attackHitbox.enabled = false;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Animation
        if (weaponAnimator != null && movement != null)
        {
            weaponAnimator.SetFloat(MoveXHash, movement.FacingX);
            weaponAnimator.SetTrigger(AttackHash);
        }

        if (attackHitbox == null)
        {
            Debug.LogError("PlayerAttack: No attackHitbox assigned.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(EnableHitbox());
    }

    private System.Collections.IEnumerator EnableHitbox()
    {
        attackHitbox.enabled = true;
        yield return new WaitForSeconds(hitboxActiveTime);
        attackHitbox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayers) == 0)
            return;

        Hitpoints hp = other.GetComponentInParent<Hitpoints>();
        if (hp == null) return;

        Vector2 knockDir = (other.transform.position - transform.position).normalized;
        hp.TakeDamage(damage, knockDir, knockbackForce);
    }
}

