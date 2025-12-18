using UnityEngine;

[CreateAssetMenu(fileName = "SO_QuestData", menuName = "Scriptable Objects/SO_QuestData")]
public class SO_QuestData : ScriptableObject
{
    public enum QuestState { open, active, completed, closed }

    public enum QuestType
    {
        CollectItem,
        KillEnemy
    }

    [Header("General Information")]
    public string questID;
    public string questName;
    public string questDescription;

    [Header("Type")]
    public QuestType questType = QuestType.CollectItem;

    [Header("Collect Requirements")]
    public SO_Item requiredItem;
    public int requiredAmount;

    [Header("Kill Requirements")]
    public string requiredEnemyID; // z.B. "Slime", "Goblin"

    [Header("Reward")]
    public int questReward;

    [Header("runtime things")]
    public int currentAmount;
    public QuestState currentState = QuestState.open;

    public bool isCompleted => currentAmount >= requiredAmount;
}

