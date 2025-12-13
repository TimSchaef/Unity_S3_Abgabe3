using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Component handling a simple movement for our enemies.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Transform target;
    private Rigidbody2D rb;

    public bool movementBlocked;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the component references we need
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (movementBlocked)
        {
            return;
        }
        
        // Move our rigidbody in the direction towards the player
        rb.linearVelocity += (Vector2) (target.position - transform.position).normalized * speed;
    }

    public void BlockMovementFor(float duration)
    {
        movementBlocked = true;

        StartCoroutine(ActivateMovementAfter(duration));
    }

    IEnumerator ActivateMovementAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        movementBlocked = false;
    }
}
