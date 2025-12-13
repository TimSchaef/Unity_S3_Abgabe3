using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestManager : MonoBehaviour
{
    [SerializeField] private Transform parentQuestEntries;
    [SerializeField] private UIQuestEntry prefabQuestEntry;

    public void UpdateAllQuestEnteries(List<SO_QuestData> allActiveQuests)
    {
        for (int i = 0; i < parentQuestEntries.childCount; i++)
        {
            Destroy(parentQuestEntries.GetChild(i).gameObject);
        }

        foreach (SO_QuestData questData in allActiveQuests)
        {
            UIQuestEntry newQuestEntry = Instantiate(prefabQuestEntry, parentQuestEntries);
            newQuestEntry.SetQuestEntry(questData);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentQuestEntries.GetComponent<RectTransform>());
    }
    
}
