using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textQuestName;
    [SerializeField] private TextMeshProUGUI textQuestDescription;
    [SerializeField] private Image imgProgressBar;
    [SerializeField] private Image imgItemIcon;
    [SerializeField] private TextMeshProUGUI textProgress;

    [Header("Optional: Icon for Kill Quests")]
    [SerializeField] private Sprite killQuestIcon; // im Inspector setzen (z.B. Skull)

    public void SetQuestEntry(SO_QuestData questdata)
    {
        textQuestName.text = questdata.questName;
        textQuestDescription.text = questdata.questDescription;

        int required = Mathf.Max(1, questdata.requiredAmount); // schützt vor /0
        int current = Mathf.Max(0, questdata.currentAmount);

        // Progress
        imgProgressBar.fillAmount = Mathf.Clamp01((float)current / required);
        textProgress.text = $"{current} / {required}";

        // Icon je nach Quest-Typ
        if (questdata.questType == SO_QuestData.QuestType.CollectItem)
        {
            if (questdata.requiredItem != null && questdata.requiredItem.itemSprite != null)
            {
                imgItemIcon.enabled = true;
                imgItemIcon.sprite = questdata.requiredItem.itemSprite;
            }
            else
            {
                // falls doch mal kein Item gesetzt ist
                imgItemIcon.enabled = false;
            }
        }
        else if (questdata.questType == SO_QuestData.QuestType.KillEnemy)
        {
            // Bei Kill-Quest: eigenes Icon oder Icon ausblenden
            if (killQuestIcon != null)
            {
                imgItemIcon.enabled = true;
                imgItemIcon.sprite = killQuestIcon;
            }
            else
            {
                imgItemIcon.enabled = false;
            }
        }
        else
        {
            imgItemIcon.enabled = false;
        }
    }
}

