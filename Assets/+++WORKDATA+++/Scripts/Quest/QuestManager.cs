using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { private set; get; }

    private List<SO_QuestData> activeQuests = new List<SO_QuestData>();

    [SerializeField] private UIQuestManager uiQuestManager;

    [Header("For Load and Save")]
    [SerializeField] private SO_QuestData[] allQuestDatas;

    private Dictionary<string, SO_QuestData> allQuests = new Dictionary<string, SO_QuestData>();

    [Header("Win")]
    [SerializeField] private GameObject winPanel;

    private bool winTriggered;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(this.gameObject); return; }
    }

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        winTriggered = false;

        allQuests.Clear();
        foreach (SO_QuestData data in allQuestDatas)
        {
            if (!allQuests.ContainsKey(data.questID))
                allQuests.Add(data.questID, data);
        }

        activeQuests.Clear();

        if (SaveManager.Instance != null && SaveManager.Instance.SaveState != null)
        {
            foreach (SaveQuestState saveQuestState in SaveManager.Instance.SaveState.saveQuestState)
            {
                if (!allQuests.ContainsKey(saveQuestState.questID))
                    continue;

                SO_QuestData newQuestData = allQuests[saveQuestState.questID];
                newQuestData.currentState = (SO_QuestData.QuestState)saveQuestState.currentState;
                newQuestData.currentAmount = saveQuestState.currentProgress;

                if (saveQuestState.currentState == (int)SO_QuestData.QuestState.active ||
                    saveQuestState.currentState == (int)SO_QuestData.QuestState.completed)
                {
                    activeQuests.Add(newQuestData);
                }
            }
        }

        uiQuestManager.UpdateAllQuestEnteries(activeQuests);

        // Nach Laden/Initialisierung direkt prüfen
        CheckWinCondition();
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetAllQuestsAndSave();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ResetAllQuestsAndSave()
    {
        // ScriptableObjects sauber zurücksetzen
        foreach (SO_QuestData data in allQuestDatas)
        {
            data.currentState = SO_QuestData.QuestState.open;
            data.currentAmount = 0;
        }

        // aktive Liste + UI zurücksetzen
        activeQuests.Clear();
        uiQuestManager.UpdateAllQuestEnteries(activeQuests);

        // Win zurücksetzen
        winTriggered = false;
        if (winPanel != null) winPanel.SetActive(false);

        // Save löschen + frischen Save anlegen
        if (SaveManager.Instance != null)
            SaveManager.Instance.DeleteSaveFile();
    }

    public void AssignQuest(SO_QuestData newQuestData)
    {
        if (newQuestData.currentState == SO_QuestData.QuestState.open)
        {
            newQuestData.currentState = SO_QuestData.QuestState.active;
            newQuestData.currentAmount = 0;
            activeQuests.Add(newQuestData);
            OnItemCollected();
        }
    }

    public void OnItemCollected()
    {
        foreach (SO_QuestData questData in activeQuests)
        {
            if (questData.questType != SO_QuestData.QuestType.CollectItem)
                continue;

            questData.currentAmount = InventoryManager.Instance.ItemCountInInventory(questData.requiredItem);

            if (questData.isCompleted)
            {
                CompleteQuest(questData);
            }
        }

        uiQuestManager.UpdateAllQuestEnteries(activeQuests);
        SaveActiveQuests();
    }

    public void OnEnemyKilled(string enemyID)
    {
        bool anyChange = false;

        foreach (SO_QuestData questData in activeQuests)
        {
            if (questData.currentState != SO_QuestData.QuestState.active)
                continue;

            if (questData.questType != SO_QuestData.QuestType.KillEnemy)
                continue;

            if (questData.requiredEnemyID != enemyID)
                continue;

            questData.currentAmount++;
            anyChange = true;

            if (questData.isCompleted)
            {
                CompleteQuest(questData);
            }
        }

        if (anyChange)
        {
            uiQuestManager.UpdateAllQuestEnteries(activeQuests);
            SaveActiveQuests();
        }
    }

    void CompleteQuest(SO_QuestData completedQuest)
    {
        completedQuest.currentState = SO_QuestData.QuestState.completed;
        uiQuestManager.UpdateAllQuestEnteries(activeQuests);
    }

    public void CloseQuest(SO_QuestData finishedQuest)
    {
        finishedQuest.currentState = SO_QuestData.QuestState.closed;

        if (finishedQuest.questType == SO_QuestData.QuestType.CollectItem)
        {
            for (int i = 0; i < finishedQuest.requiredAmount; i++)
            {
                InventoryManager.Instance.RemoveItem(finishedQuest.requiredItem);
            }
        }

        activeQuests.Remove(finishedQuest);
        uiQuestManager.UpdateAllQuestEnteries(activeQuests);
        SaveActiveQuests();

        // Nach dem Schließen prüfen, ob alle Quests abgeschlossen sind
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (winTriggered) return;

        // Optional: wenn keine Quests definiert sind, nicht gewinnen
        if (allQuestDatas == null || allQuestDatas.Length == 0) return;

        for (int i = 0; i < allQuestDatas.Length; i++)
        {
            if (allQuestDatas[i].currentState != SO_QuestData.QuestState.closed)
                return;
        }

        winTriggered = true;
        if (winPanel != null) winPanel.SetActive(true);
        Debug.Log("WIN: Alle Quests sind abgeschlossen.");
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
                newSaveQuestState.currentState = (int)allQuestDatas[i].currentState;
                newSaveQuestState.currentProgress = allQuestDatas[i].currentAmount;

                temporaryQuestList.Add(newSaveQuestState);
            }
        }

        SaveManager.Instance.SaveState.saveQuestState = temporaryQuestList.ToArray();
        SaveManager.Instance.SaveGame();
    }
}


