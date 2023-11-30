using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    // Handles multiple inventories and when the mouse is not on an inventory
    public Inventory selectedInventory;

    // Selected (held) item
    [SerializeField] InventoryItem selectedItem;
    // Used to store an overlapping item
    InventoryItem overlapItem;
    RectTransform selectedRectTransform;

    void Update()
    {
        // Move the selected item to the mouse
        DragItem();

        // Stops inventory interaction when no inventory is selected
        if (selectedInventory == null)
            return;

        // Perform left mouse button event
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (selectedItem == null) return;
            selectedItem.Rotate();
        }
    }

    // Performs place/pickup 
    private void LeftMouseButtonPress()
    {
        Vector2 position = Input.mousePosition;

        // Used to offset mouse click position with selected item dimensions
        // Even though the selected item is dragged by its center, clicking will occur in its top left tile
        if (selectedItem != null)
        {
            position.x -= (selectedItem.sizeWidth - 1) * Inventory.tileDimension / 2;
            position.y += (selectedItem.sizeHeight - 1) * Inventory.tileDimension / 2;
        }

        // Find Coordinates on inventory for mouse click position
        Vector2Int tileClickPosition = selectedInventory.getTilePosition(position);

        // If no item is selected, attempt to pick up item
        if (selectedItem == null)
        {
            PickUpItem(tileClickPosition);
        }
        // If item is selected, attempt to place item
        else
        {
            PlaceItem(tileClickPosition);
        }
    }

    // Places item at coordinate
    private void PlaceItem(Vector2Int tileClickPosition)
    {
        // Attempts to place item
        bool placed = selectedInventory.PlaceItem(ref selectedItem, tileClickPosition.x, tileClickPosition.y, ref overlapItem);
        // If successfully placed
        if (placed) {
            // Set selected item to null to show an empty hand
            selectedItem = null;
            // If it finds a singular overlapItem, swap the two items
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                selectedRectTransform = selectedItem.GetComponent<RectTransform>();
                selectedRectTransform.SetAsLastSibling();
            }
        }
    }

    // Picks up item at coordinate
    private void PickUpItem(Vector2Int tileClickPosition)
    {
        // Sets selected item to whatever item is at the coordinates
        selectedItem = selectedInventory.PickUpItem(tileClickPosition.x, tileClickPosition.y);
        if (selectedItem != null)
        {
            selectedRectTransform = selectedItem.GetComponent<RectTransform>();
            selectedRectTransform.SetAsLastSibling();
        }
    }

    // Moves the selected item to the mouse position
    private void DragItem()
    {
        if (selectedItem != null)
            selectedRectTransform.position = Input.mousePosition;
    }
}
