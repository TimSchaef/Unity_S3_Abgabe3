using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private SO_QuestData questData;
    
    [SerializeField] private GameObject highlight;

    private PlayerInteractions playerInteractions;

    [SerializeField] private TextMeshProUGUI textQuestState;

    private SO_QuestData.QuestState questStateLastFrame;
    
    private void Start()
    {
        highlight.SetActive(false);
        playerInteractions = FindFirstObjectByType<PlayerInteractions>();

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
        if (questData.currentState == SO_QuestData.QuestState.open)
        {
            textQuestState.text = "?";
            textQuestState.transform.DOScale(1.3f, 1f).
                SetLoops(-1, LoopType.Yoyo).
                SetEase(Ease.InOutCubic);
            //SetId(questData.questID);
        }else if (questData.currentState == SO_QuestData.QuestState.active)
        {
            textQuestState.text = "...";
            textQuestState.transform.DOKill();
            //DOTween.Kill(questData.questID);
            //DOTween.Kill(textQuestState.transform);

        }else if (questData.currentState == SO_QuestData.QuestState.completed)
        {
            textQuestState.text = "!";
            textQuestState.transform.DOLocalMoveY(120f, 1f).
                SetLoops(-1, LoopType.Yoyo).
                SetEase(Ease.OutCubic);
            textQuestState.transform.DOScale(1.3f, 1f).
                SetLoops(-1, LoopType.Yoyo).
                SetEase(Ease.InOutCubic);
        }else if (questData.currentState == SO_QuestData.QuestState.closed)
        {
            textQuestState.text = " ";
            textQuestState.transform.DOKill();
        }
    }

    private void OnPlayerinteraction()
    {
        if (questData.currentState == SO_QuestData.QuestState.open)
        {
            QuestManager.Instance.AssignQuest(questData);
        }else if (questData.currentState == SO_QuestData.QuestState.completed)
        {
            QuestManager.Instance.CloseQuest(questData);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player is near " + gameObject.name);
        //hier soll es als einsammelbar markiert werden
        if (other.CompareTag("Player"))
        {
            highlight.SetActive(true);
            
            playerInteractions.OnInteract.AddListener(OnPlayerinteraction);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player left area of " + gameObject.name);
        //hier soll es nicht mehr einsammelbar sein
        if (other.CompareTag("Player"))
        {
            highlight.SetActive(false);
            
            playerInteractions.OnInteract.RemoveListener(OnPlayerinteraction);
        }
    }
}
