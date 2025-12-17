using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 2f;

    [Header("Interval Damage")]
    [SerializeField] private float damageInterval = 0.5f;

    private float damageTimer;
    private Hitpoints currentTarget;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Hitpoints hitpoints = other.gameObject.GetComponent<Hitpoints>();
        if (hitpoints == null) return;

        // Sofort Schaden beim ersten Kontakt (wie vorher)
        currentTarget = hitpoints;
        damageTimer = 0f;

        Vector2 dir = other.transform.position - transform.position;
        hitpoints.TakeDamage(damage, dir, knockbackForce);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Hitpoints>() == currentTarget)
        {
            currentTarget = null;
        }
    }

    private void Update()
    {
        if (currentTarget == null) return;

        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            damageTimer = 0f;

            Vector2 dir = currentTarget.transform.position - transform.position;
            currentTarget.TakeDamage(damage, dir, knockbackForce);
        }
    }
}

