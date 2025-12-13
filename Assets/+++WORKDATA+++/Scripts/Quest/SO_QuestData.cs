using UnityEngine;

[CreateAssetMenu(fileName = "SO_QuestData", menuName = "Scriptable Objects/SO_QuestData")]
public class SO_QuestData : ScriptableObject
{
    public enum QuestState
    {
        open,
        active,
        completed,
        closed
    }

    [Header("General Information")]
    public string questID;
    public string questName;
    public string questDescription;

    [Header("Requirements")]
    public SO_Item requiredItem;
    public int requiredAmount;

    [Header("Reward")]
    public int questReward; //in gold
    
    [Header("runtime things")]
    public int currentAmount;
    public QuestState currentState = QuestState.open;

    public bool isCompleted => currentAmount >= requiredAmount;
}
