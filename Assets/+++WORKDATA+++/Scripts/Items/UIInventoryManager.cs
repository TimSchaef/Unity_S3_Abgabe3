using System;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{
    [SerializeField] private UIElementItem prefabElementItem;
    [SerializeField] private Transform parentItems;

    [SerializeField] private GameObject panelInventoryUI;

    private bool isInventoryOpen = false;
    
    private void Start()
    {
        panelInventoryUI.SetActive(false);
    }

    public void ToggleInventory()
    {
        if (isInventoryOpen)
        {
            HideInventory();
            isInventoryOpen = false;
        }
        else
        {
            ShowInventory();
            isInventoryOpen = true;
        }
    }

    public void HideInventory()
    {
        panelInventoryUI.SetActive(false);
    }

    public void ShowInventory()
    {
        panelInventoryUI.SetActive(true);

        foreach (Transform oldItemElementsUI in parentItems)
        {
            Destroy(oldItemElementsUI.gameObject);
        }

        foreach (SO_Item item in InventoryManager.Instance.ItemsInInventory)
        {
            UIElementItem newItemElementUI = Instantiate(prefabElementItem, parentItems);
            newItemElementUI.SetContent(item.itemName, item.itemSprite);
        }
    }
}
