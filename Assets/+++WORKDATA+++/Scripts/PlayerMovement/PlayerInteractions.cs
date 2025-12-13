using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    public UnityEvent OnInteract;
    
    public void Interact(InputAction.CallbackContext context)
    {
        //wenn es im InventoryManager collectable Items gibt, sammle sie ein
        InventoryManager.Instance.TryCollectItems();
        
        OnInteract?.Invoke();
    }

    public void Inventory(InputAction.CallbackContext context)
    {
        //nur sehr sparsam bei einzelnen Aufrufen butzen! Rechenintensiv!
        //Wir durchsuchen die ganze Szene nach einem UIInventoryManager -> wenn der erste gefunden wird dann bricht ab
        FindFirstObjectByType<UIInventoryManager>().ToggleInventory();
    }
}
