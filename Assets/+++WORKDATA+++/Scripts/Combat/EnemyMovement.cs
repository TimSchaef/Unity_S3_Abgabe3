using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float loseRadiusBuffer = 0.5f; // Hysterese

    private Transform target;
    private Rigidbody2D rb;
    private Animator animator;

    public bool movementBlocked;

    private bool isChasing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        TryFindPlayer();
    }

    private void FixedUpdate()
    {
        if (movementBlocked)
        {
            StopMovement();
            return;
        }

        if (target == null)
        {
            TryFindPlayer();
            StopMovement();
            return;
        }

        float dist = Vector2.Distance(rb.position, (Vector2)target.position);

        // Chase-State setzen
        if (!isChasing && dist <= detectionRadius)
            isChasing = true;

        // Chase-State beenden erst mit Puffer
        if (isChasing && dist > detectionRadius + loseRadiusBuffer)
            isChasing = false;

        if (!isChasing)
        {
            StopMovement();
            return;
        }

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;

        // Verlässlicher für Rigidbody2D
        rb.linearVelocity = dir * speed;

        animator.SetBool("IsMoving", rb.linearVelocity.sqrMagnitude > 0.01f);
    }

    private void TryFindPlayer()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        target = playerObj != null ? playerObj.transform : null;
    }

    private void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }

    public void BlockMovementFor(float duration)
    {
        movementBlocked = true;
        StopMovement();
        StartCoroutine(ActivateMovementAfter(duration));
    }

    private IEnumerator ActivateMovementAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        movementBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius + loseRadiusBuffer);
    }
}



