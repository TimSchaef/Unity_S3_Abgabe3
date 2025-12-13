using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Provides the attack ability of the player character.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRadius;
    [SerializeField] private float knockbackForce;
    
    [SerializeField] private GameObject attackVisualizer;
    [SerializeField] private Transform turnPivot;
    
    
    
    /// <summary>
    /// Attack function that is called by the Input System. Deals damage to anything in front of the character with an OverlapCircle.
    /// </summary>
    public void Attack(InputAction.CallbackContext context)
    {
        // Only react to the completed button press (not on starting or ending)
        if  (context.performed)
        {
            // Spawn an OverlapCircle in front of the character and save all hit colliders in an array
            Collider2D[] hits = Physics2D.OverlapCircleAll(turnPivot.position + turnPivot.up * attackRange, attackRadius);
            
            // Visualize the attack range by activating a GameObject
            attackVisualizer.SetActive(true);
            // Deactivate the same GameObject again after 200 ms with a Coroutine
            StartCoroutine(DeactivateAttackVisualizer(0.2f));
            
            // Loop through all colliders that have been found
            foreach (Collider2D hit in hits)
            {
                // Check if the collider has a Hitpoints component attached to it
                // Also make sure we aren't reacting to our own collider (and damaging ourselves)
                Hitpoints hitpoints = hit.GetComponent<Hitpoints>();
                if (hitpoints != null && hitpoints.gameObject != gameObject)
                {
                    // Deal damage to the found Hitpoints component by calling its function
                    Debug.Log("Dealing damage to: " + hit.name);
                    hitpoints.TakeDamage(damage, hit.transform.position - transform.position, knockbackForce);
                }
            }
        }
    }

    /// <summary>
    /// Deactivates the attack visualizer GameObject after the given delay (in seconds).
    /// </summary>
    IEnumerator DeactivateAttackVisualizer(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        attackVisualizer.SetActive(false);
    }
}
