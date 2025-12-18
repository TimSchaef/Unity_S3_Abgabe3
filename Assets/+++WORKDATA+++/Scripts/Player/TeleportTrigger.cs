using System.Collections;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Teleport")]
    [SerializeField] private Transform destination;
    [SerializeField] private float cooldown = 0.5f;

    [Header("UI (optional)")]
    [SerializeField] private GameObject panel;
    [SerializeField] private float panelDuration = 1.0f;

    [Header("Block movement (optional)")]
    [SerializeField] private bool blockMovementWhilePanelShows = true;

    private bool coolingDown;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (coolingDown) return;
        if (destination == null) return;

        
        var playerMovement = other.GetComponentInParent<PlayerMovement>();
        if (playerMovement == null) return;

        var rb = playerMovement.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        StartCoroutine(TeleportRoutine(playerMovement, rb));
    }

    private IEnumerator TeleportRoutine(PlayerMovement pm, Rigidbody2D rb)
    {
        coolingDown = true;

        if (panel != null)
            panel.SetActive(true);

        if (blockMovementWhilePanelShows)
            pm.BlockMovementFor(panelDuration);

        
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

       
        rb.position = destination.position;
        rb.rotation = destination.eulerAngles.z;
        

        yield return new WaitForSecondsRealtime(panelDuration);

        if (panel != null)
            panel.SetActive(false);

        yield return new WaitForSecondsRealtime(cooldown);
        coolingDown = false;
    }
}
