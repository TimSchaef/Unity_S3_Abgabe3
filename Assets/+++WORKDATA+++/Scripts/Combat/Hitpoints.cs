using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

/// <summary>
/// Component that provides Hitpoints to an entity and thereby makes it also vulnerable to attacks.
/// </summary>
public class Hitpoints : MonoBehaviour
{
    [SerializeField] private int hitpoints;
    [SerializeField] private UIHitpoints uiHitpoints;
    [SerializeField] private AudioClip hitSound;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CinemachineImpulseSource cinemachineImpulseSource;


    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cinemachineImpulseSource
            = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary>
    /// Registers damage dealt and handles all effects of it.
    /// </summary>
    /// <param name="damage">The damage dealt</param>
    /// <param name="knockbackDirection">The direction the knockback should be applied in</param>
    /// <param name="knockbackForce">The force of the knockback</param>
    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce)
    {
        // Reduce our hitpoints
        hitpoints = hitpoints - damage;

        // Apply the knockback to our Rigidbody2D
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (playerMovement != null)
        {
            playerMovement.BlockMovementFor(0.5f);
        }
        
        // Update our UI element, but only if one was linked to this component in the Inspector
        if (uiHitpoints != null)
        {
            uiHitpoints.UpdateHitpoints(hitpoints);
        }

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.DOKill(true);
            spriteRenderer.DOColor(Color.red, 0.2f).SetLoops(2, LoopType.Yoyo);
        }

        if (gameObject.CompareTag("Player"))
        {
            // Camera.main.GetComponent<PositionConstraint>().enabled = false;
            // Camera.main.DOKill(true);
            // Camera.main.DOShakePosition(0.3f, 1f).OnComplete(() =>
            // {
            //     Camera.main.GetComponent<PositionConstraint>().enabled = true;
            // });
            
            cinemachineImpulseSource.GenerateImpulse();
        }
        
        // Are we dead yet?
        if (hitpoints <= 0)
        {
            // If we are the player => Reload the scene to reset everything
            if (gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            // Otherwise => Just destroy ourselves
            else
            {
                spriteRenderer.DOKill();
                Destroy(gameObject);
            }
        }
    }
}
