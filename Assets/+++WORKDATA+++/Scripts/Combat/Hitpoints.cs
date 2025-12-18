using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Hitpoints : MonoBehaviour
{
    [Header("Hitpoints")]
    [SerializeField] private int hitpoints = 3;

    [Header("UI (optional)")]
    [SerializeField] private UIHitpoints uiHitpoints;

    [Header("Audio (optional)")]
    [SerializeField] private AudioClip hitSound;

    [Header("Quest (Enemy)")]
    [Tooltip("Enable if this object should count for kill-quests.")]
    [SerializeField] private bool countsAsEnemyForQuests = true;

    [Tooltip("Identifier used by kill-quests, e.g. 'Slime', 'Goblin'.")]
    [SerializeField] private string enemyID = "Slime";

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CinemachineImpulseSource cinemachineImpulseSource;

    private bool isDead;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>(); // usually only on Player
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

   
    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce)
    {
        if (isDead) return;

        hitpoints -= damage;

        // Knockback
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        
        if (playerMovement != null)
        {
            playerMovement.BlockMovementFor(0.5f);
        }

        
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

        
        if (CompareTag("Player") && cinemachineImpulseSource != null)
        {
            cinemachineImpulseSource.GenerateImpulse();
        }

        
        if (hitpoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        
        if (CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        
        if (spriteRenderer != null)
        {
            spriteRenderer.DOKill();
        }
        
        gameObject.SetActive(false);
        Destroy(gameObject);
        
        if (countsAsEnemyForQuests && !string.IsNullOrWhiteSpace(enemyID))
        {
            try
            {
                if (QuestManager.Instance != null)
                {
                    QuestManager.Instance.OnEnemyKilled(enemyID);
                }
                else
                {
                    Debug.LogWarning($"QuestManager.Instance is null (enemyID={enemyID})");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"OnEnemyKilled failed for enemyID={enemyID}: {ex}");
            }
        }
    }

    public string EnemyID => enemyID;
}

