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

    public void SetQuestEntry(SO_QuestData questdata)
    {
        textQuestName.text = questdata.questName;
        textQuestDescription.text = questdata.questDescription;
        //progressbar
        imgProgressBar.fillAmount = (float) questdata.currentAmount/questdata.requiredAmount;
        
        //itemicon
        imgItemIcon.sprite = questdata.requiredItem.itemSprite;
        
        textProgress.text = questdata.currentAmount + " / " + questdata.requiredAmount;
    }
}
