using System;
using UnityEngine;

[Serializable]
public class GlobalSaveState
{
    public SaveQuestState[] saveQuestState = new SaveQuestState[0];
    public string[] inventoryItems = new string[0];
    public string[] collectedItemIDs = new string[0];
}
