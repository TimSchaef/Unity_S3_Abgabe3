using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hitpoints : MonoBehaviour
{
    [Header("Hitpoints")]
    [SerializeField] private int maxHitpoints = 3;
    [SerializeField] private int hitpoints = 3;

    [Header("Enemy Settings (optional)")]
    [SerializeField] private bool isEnemy;
    [SerializeField] private string enemyID = "Slime";

    [Header("UI (Fill Image)")]
    [SerializeField] private UIHealthBarFill uiHealthBar;

    [Header("Audio (optional)")]
    [SerializeField] private AudioClip hitSound;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CinemachineImpulseSource cinemachineImpulseSource;

    private bool isDead;

    public int CurrentHP => hitpoints;
    public int MaxHP => maxHitpoints;

    private void Awake()
    {
        maxHitpoints = Mathf.Max(1, maxHitpoints);
        hitpoints = Mathf.Clamp(hitpoints, 0, maxHitpoints);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        // Player HP aus Save laden
        if (!isEnemy && CompareTag("Player") && SaveManager.Instance != null)
        {
            int loadedHP = SaveManager.Instance.SaveState.saveHP;
            hitpoints = loadedHP > 0
                ? Mathf.Clamp(loadedHP, 0, maxHitpoints)
                : maxHitpoints;
        }

        UpdateUI();
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce)
    {
        if (isDead) return;

        hitpoints -= damage;
        hitpoints = Mathf.Clamp(hitpoints, 0, maxHitpoints);

        if (rb != null)
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        if (!isEnemy && playerMovement != null)
            playerMovement.BlockMovementFor(0.5f);

        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (spriteRenderer != null)
        {
            spriteRenderer.DOKill(true);
            spriteRenderer.DOColor(Color.red, 0.2f).SetLoops(2, LoopType.Yoyo);
        }

        if (!isEnemy && cinemachineImpulseSource != null)
            cinemachineImpulseSource.GenerateImpulse();

        UpdateUI();

        if (hitpoints <= 0)
            Die();
    }

    private void UpdateUI()
    {
        if (uiHealthBar != null)
            uiHealthBar.SetHP(hitpoints, maxHitpoints);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // ===============================
        // PLAYER DEATH
        // ===============================
        if (!isEnemy && CompareTag("Player"))
        {
            // HP zurück auf MAX setzen
            hitpoints = maxHitpoints;

            // Save aktualisieren
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveState.saveHP = maxHitpoints;
                SaveManager.Instance.SaveGame();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        // ===============================
        // ENEMY DEATH
        // ===============================
        if (isEnemy && QuestManager.Instance != null)
        {
            QuestManager.Instance.OnEnemyKilled(enemyID);
        }

        Destroy(gameObject);
    }

    public void SetHP(int value)
    {
        hitpoints = Mathf.Clamp(value, 0, maxHitpoints);
        UpdateUI();

        if (hitpoints <= 0 && !isDead)
            Die();
    }
}


