using System;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public SO_Item itemData;

    [SerializeField] private GameObject highlight;

    public string GUID; 

    private void Start()
    {
        highlight.SetActive(false);
    }
    
    [ContextMenu("GenerateGUID")]
    void CreateGUID()
    {
        GUID = Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player is near " + gameObject.name);
        //hier soll es als einsammelbar markiert werden
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.collectableItems.Add(this);
            highlight.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player left area of " + gameObject.name);
        //hier soll es nicht mehr einsammelbar sein
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.collectableItems.Remove(this);
            highlight.SetActive(false);
        }
    }
}
