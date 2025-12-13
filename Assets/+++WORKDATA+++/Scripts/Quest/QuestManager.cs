using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private List<SO_QuestData> activeQuests = new List<SO_QuestData>();

    [SerializeField] private UIQuestManager uiQuestManager;

    [Header("For Load and Save")]
    [SerializeField] private SO_QuestData[] allQuestDatas;

    private Dictionary<string, SO_QuestData> allQuests = new Dictionary<string, SO_QuestData>();

    private void Start()
    {
        foreach (SO_QuestData data in allQuestDatas)
        {
            allQuests.Add(data.questID, data);
        }

        foreach (SaveQuestState saveQuestState in SaveManager.Instance.SaveState.saveQuestState)
        {
            SO_QuestData newQuestData = allQuests[saveQuestState.questID];
            newQuestData.currentState = (SO_QuestData.QuestState) saveQuestState.currentState;
            newQuestData.currentAmount = saveQuestState.currentProgress;
            
            if (saveQuestState.currentState == (int)SO_QuestData.QuestState.active ||
                saveQuestState.currentState == (int)SO_QuestData.QuestState.completed)
            {
                activeQuests.Add(newQuestData);
            }
        }
        
        uiQuestManager.UpdateAllQuestEnteries(activeQuests);
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            foreach (SO_QuestData data in allQuestDatas)
            {
                data.currentState = SO_QuestData.QuestState.open;
            }

            SaveManager.Instance.DeleteSaveFile();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }

    public void AssignQuest(SO_QuestData newQuestData)
    {
        if (newQuestData.currentState == SO_QuestData.QuestState.open)
        {
            newQuestData.currentState = SO_QuestData.QuestState.active;
            newQuestData.currentAmount = 0;
            activeQuests.Add(newQuestData);
            //update quest ui -> add quest as active
            OnItemCollected();
        }
    }

    public void OnItemCollected()
    {
        foreach (SO_QuestData questData in activeQuests)
        {
            questData.currentAmount = InventoryManager.Instance.ItemCountInInventory(questData.requiredItem);
            if (questData.isCompleted)
            {
                CompleteQuest(questData);
            }
            else
            {
                //quest ui updaten with new amount
            }
        }
        
        uiQuestManager.UpdateAllQuestEnteries(activeQuests);
        SaveActiveQuests();
    }

    void CompleteQuest(SO_QuestData completedQuest)
    {
        completedQuest.currentState = SO_QuestData.QuestState.completed;
        //quest UI update -> quest completed and can be returned to NPC
        
        uiQuestManager.UpdateAllQuestEnteries(activeQuests);
    }

    public void CloseQuest(SO_QuestData finishedQuest)
    {
        finishedQuest.currentState = SO_QuestData.QuestState.closed;
        
        //Items aus Inventar entfernen
        for (int i = 0; i < finishedQuest.requiredAmount; i++)
        {
            InventoryManager.Instance.RemoveItem(finishedQuest.requiredItem);
        }
        
        //receive reward -> finishedQuest.questReward
        activeQuests.Remove(finishedQuest);
        //update quest ui -> remove finished Quest from list
        
        uiQuestManager.UpdateAllQuestEnteries(activeQuests);
        SaveActiveQuests();
    }

    void SaveActiveQuests()
    {
        List<SaveQuestState> temporaryQuestList = new List<SaveQuestState>();

        for (int i = 0; i < allQuestDatas.Length; i++)
        {
            if (allQuestDatas[i].currentState != SO_QuestData.QuestState.open)
            {
                SaveQuestState newSaveQuestState = new SaveQuestState();
                newSaveQuestState.questID = allQuestDatas[i].questID;
                newSaveQuestState.currentState = (int) allQuestDatas[i].currentState;
                newSaveQuestState.currentProgress = allQuestDatas[i].currentAmount;

                temporaryQuestList.Add(newSaveQuestState);
            }
        }
        
        SaveManager.Instance.SaveState.saveQuestState = temporaryQuestList.ToArray();
        
        SaveManager.Instance.SaveGame();
    }
}
