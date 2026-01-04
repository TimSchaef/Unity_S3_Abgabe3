using DG.Tweening;
using TMPro;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private SO_QuestData questData;
    [SerializeField] private TextAsset inkQuestText;
    [SerializeField] private GameObject highlight;
    [SerializeField] private TextMeshProUGUI textQuestState;

    private PlayerInteractions playerInteractions;
    private SO_QuestData.QuestState questStateLastFrame;

    private bool playerInRange;
    private bool waitingForDialogueEnd;

    private InkDialogue inkDialogue;

    private void Start()
    {
        if (highlight != null) highlight.SetActive(false);

        playerInteractions = FindFirstObjectByType<PlayerInteractions>();
        if (playerInteractions == null)
            Debug.LogError("QuestGiver: PlayerInteractions nicht gefunden.");

        inkDialogue = FindFirstObjectByType<InkDialogue>();
        if (inkDialogue == null)
            Debug.LogError("QuestGiver: InkDialogue nicht in der Szene gefunden.");

        questStateLastFrame = questData.currentState;
        UpdateTextQuestState();
    }

    private void Update()
    {
        if (questData.currentState != questStateLastFrame)
        {
            UpdateTextQuestState();
            questStateLastFrame = questData.currentState;
        }
    }

    private void UpdateTextQuestState()
    {
        if (textQuestState == null) return;

        textQuestState.transform.DOKill();

        if (questData.currentState == SO_QuestData.QuestState.open)
        {
            textQuestState.text = "?";
            textQuestState.transform.DOScale(1.3f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);
        }
        else if (questData.currentState == SO_QuestData.QuestState.active)
        {
            textQuestState.text = "...";
        }
        else if (questData.currentState == SO_QuestData.QuestState.completed)
        {
            textQuestState.text = "!";
            textQuestState.transform.DOLocalMoveY(120f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.OutCubic);
            textQuestState.transform.DOScale(1.3f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);
        }
        else if (questData.currentState == SO_QuestData.QuestState.closed)
        {
            textQuestState.text = " ";
        }
    }

    private void OnPlayerinteraction()
    {
        if (!playerInRange) return;
        if (inkDialogue == null) return;

        // WICHTIG: Wenn Dialog läuft, soll E trotzdem funktionieren -> weiterklicken
        if (inkDialogue.IsActive)
        {
            inkDialogue.Continue(); // Continue-Button bleibt zusätzlich nutzbar
            return;
        }

        // Wenn wir auf Dialog-Ende warten (Quest-Action kommt gleich): nicht neu starten
        if (waitingForDialogueEnd) return;

        // Dialog starten (E anspricht den NPC)
        if (questData.currentState == SO_QuestData.QuestState.open)
        {
            StartNpcDialogueThen(() => QuestManager.Instance.AssignQuest(questData));
        }
        else if (questData.currentState == SO_QuestData.QuestState.completed)
        {
            StartNpcDialogueThen(() => QuestManager.Instance.CloseQuest(questData));
        }
        else
        {
            // Optional: auch in anderen States sprechen lassen
            StartNpcDialogueThen(null);
        }
    }

    private void StartNpcDialogueThen(System.Action afterDialogue)
    {
        if (inkQuestText == null)
        {
            Debug.LogError($"QuestGiver: Ink JSON (inkQuestText) ist nicht gesetzt bei {name}.");
            return;
        }

        inkDialogue.SetInk(inkQuestText);

        waitingForDialogueEnd = true;
        StartCoroutine(WaitForDialogueEndCoroutine(afterDialogue));

        inkDialogue.StartDialogue();
    }

    private System.Collections.IEnumerator WaitForDialogueEndCoroutine(System.Action afterDialogue)
    {
        while (inkDialogue != null && !inkDialogue.IsActive)
            yield return null;

        while (inkDialogue != null && inkDialogue.IsActive)
            yield return null;

        waitingForDialogueEnd = false;
        afterDialogue?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (highlight != null) highlight.SetActive(true);

        if (playerInteractions != null)
            playerInteractions.OnInteract.AddListener(OnPlayerinteraction);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (highlight != null) highlight.SetActive(false);

        if (playerInteractions != null)
            playerInteractions.OnInteract.RemoveListener(OnPlayerinteraction);
    }
}







