using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private Transform turnPivot;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Vector2 moveInput;

    private bool movementBlocked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(rb== null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        if (movementBlocked)
        {
            return;
        }
        
        rb.linearVelocity = moveInput * (moveSpeed + InventoryManager.Instance.GetBonusSpeed());

        if (rb.linearVelocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.linearVelocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        
        // Is the player giving any movement input right now?
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            // If yes: Rotate the character in the movement direction
            turnPivot.rotation = Quaternion.LookRotation(Vector3.forward, rb.linearVelocity);
        }
    }*/

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void BlockMovementFor(float duration)
    {
        movementBlocked = true;

        StartCoroutine(ActivateMovementAfter(duration));
    }

    IEnumerator ActivateMovementAfter(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        
        movementBlocked = false;
    }
}
