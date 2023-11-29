using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Controller that detects inputs
    [SerializeField] InventoryController inventoryController;
    // Inventory for which this handles interactions
    Inventory inventory;

    // On mouse enter, set this inventory as the selected inventory
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.selectedInventory = inventory;
    }

    // On mouse exist, set the selected inventory as null
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.selectedInventory = null;
    }

    void Awake()
    {
        inventory = GetComponent<Inventory>();
    }
}
