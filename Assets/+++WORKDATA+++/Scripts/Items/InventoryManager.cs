using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<SO_Item> ItemsInInventory = new List<SO_Item>();

    //hier werden die Items, welche jetzt gerade einsammelbar sind, zwischengespeichert
    public List<CollectableItem> collectableItems = new List<CollectableItem>();

    [SerializeField] private SO_Item[] allPossibleItems;
    private Dictionary<string, SO_Item> allItems = new Dictionary<string, SO_Item>();

    private List<string> collectedItemIDs = new List<string>();
    
    //singelton initialisierung
    public static InventoryManager Instance { private set; get;}

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

    private void Start()
    {
        foreach (SO_Item newDicEntry in allPossibleItems)
        {
            allItems.Add(newDicEntry.itemID, newDicEntry);
        }

        foreach (string itemID in SaveManager.Instance.SaveState.inventoryItems)
        {
            ItemsInInventory.Add(allItems[itemID]);
        }

        foreach (string collectedItemID in SaveManager.Instance.SaveState.collectedItemIDs)
        {
            collectedItemIDs.Add(collectedItemID);
        }

        foreach (CollectableItem collectableItemInScene in FindObjectsByType<CollectableItem>(
                     FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (collectedItemIDs.Contains(collectableItemInScene.GUID))
            {
                Destroy(collectableItemInScene.gameObject);
            }
        }
    }

    public void TryCollectItems()
    {
        //für jedes einsammelbare Item (in der Nähe)
        for (int i = 0; i < collectableItems.Count; i++)
        {
            collectedItemIDs.Add(collectableItems[i].GUID);
            
            //hole dir die Daten des Items und gebe sie der Funktion 'AddItem' mit
            AddItem(collectableItems[i].itemData);
            //Das gameObject des Items in der Szene löschen...ist ja jetzt eingesammelt
            Destroy(collectableItems[i].gameObject);
        }
    }

    public void AddItem(SO_Item newItem)
    {
        //nehme die übergebenen ItemDaten und füge sie der Inventarliste hinzu
        ItemsInInventory.Add(newItem);
        QuestManager.Instance.OnItemCollected();
        
        SaveInventory();
    }

    public void RemoveItem(SO_Item itemToRemove)
    {
        if (ItemsInInventory.Contains(itemToRemove))
        {
            ItemsInInventory.Remove(itemToRemove);
        
            SaveInventory();
        }
        else
        {
            Debug.LogWarning(itemToRemove.itemID + " No such Item in Inventory");
        }
    }

    public int ItemCountInInventory(SO_Item itemData)
    {
        int counter = 0;
        foreach (SO_Item item in ItemsInInventory)
        {
            if (item == itemData)
            {
                counter++;
            }
        }
        return counter;
    }

    void SaveInventory()
    {
        SaveManager.Instance.SaveState.inventoryItems = new string[ItemsInInventory.Count];

        int index = 0;
        foreach (SO_Item item in ItemsInInventory)
        {
            SaveManager.Instance.SaveState.inventoryItems[index] = item.itemID;

            index++;
        }

        SaveManager.Instance.SaveState.collectedItemIDs = collectedItemIDs.ToArray();
        
        SaveManager.Instance.SaveGame();
    }
}
