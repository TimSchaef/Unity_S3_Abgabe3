using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;

    [Header("Animation (Player)")]
    [SerializeField] private Animator animator;

    private Vector2 moveInput;
    private bool movementBlocked;

  
    [SerializeField] private float lastMoveX = 1f;
    private const float deadzone = 0.15f;

    public float FacingX => lastMoveX;

    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int MoveXHash    = Animator.StringToHash("MoveX");

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (movementBlocked)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool(IsMovingHash, false);
            animator.SetFloat(MoveXHash, lastMoveX);
            return;
        }
        
        Vector2 input = (moveInput.magnitude >= deadzone) ? moveInput : Vector2.zero;

        
        rb.linearVelocity = input * moveSpeed;

        bool isMoving = input.sqrMagnitude > 0.0001f;
        animator.SetBool(IsMovingHash, isMoving);

        
        if (input.x > 0.01f) lastMoveX = 1f;
        else if (input.x < -0.01f) lastMoveX = -1f;

        animator.SetFloat(MoveXHash, lastMoveX);
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void BlockMovementFor(float duration)
    {
        if (!gameObject.activeInHierarchy) return;

        movementBlocked = true;
        StopAllCoroutines();
        StartCoroutine(UnblockAfter(duration));
    }

    private IEnumerator UnblockAfter(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        movementBlocked = false;
    }
}



