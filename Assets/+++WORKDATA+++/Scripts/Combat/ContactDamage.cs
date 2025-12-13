using System;
using UnityEngine;

/// <summary>
/// This component deals damage on a collision to any entity that has Hitpoints.
/// </summary>
public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float knockbackForce;
    
    
    
    // Being called when a collision with another collider occurs
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the hit collider has a Hitpoints component attached to it
        Hitpoints hitpoints = other.gameObject.GetComponent<Hitpoints>();
        if (hitpoints != null)
        {
            // If yes: Deal damage by calling its TakeDamage function
            // Also provide the direction and force of the knockback that should be applied
            hitpoints.TakeDamage(damage, other.transform.position - transform.position, knockbackForce);
        }
    }
}
