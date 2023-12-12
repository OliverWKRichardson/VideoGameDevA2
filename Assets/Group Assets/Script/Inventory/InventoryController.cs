using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    // Handles multiple inventories and when the mouse is not on an inventory
    [HideInInspector] public Inventory selectedInventory;

    [SerializeField] Inventory mainInventory;
    [SerializeField] Inventory groundInventory;
    private InventoryInteract mainInventoryInteract;
    private InventoryInteract groundInventoryInteract;
    [SerializeField] GameObject contextMenu;
    contextMenuController contextMenuCont;

    // Manages dropping items
    [SerializeField] PickupController pickupController;

    bool inventoryActive = false;

    // Selected (held) item
    InventoryItem selectedItem;
    // Used to store an overlapping item
    InventoryItem overlapItem;
    RectTransform selectedRectTransform;

    // Equipped Item (Weapon)
    [HideInInspector] public InventoryItem equippedItem;

    // Item that the context menu is referencing
    InventoryItem rightClickItem;

    // Used for crafting
    [SerializeField] GameObject basicGunPrefab;
    [SerializeField] GameObject betterGunPrefab;

    [SerializeField] Canvas canvas;

    void Awake()
    {
        contextMenuCont = contextMenu.GetComponent<contextMenuController>();
        mainInventoryInteract = mainInventory.gameObject.GetComponent<InventoryInteract>();
        groundInventoryInteract = groundInventory.gameObject.GetComponent<InventoryInteract>();
    }

    void Update()
    {
        // If the inventory has just been closed
        if (inventoryActive && !mainInventory.gameObject.activeInHierarchy) { 
            // Disable the InventoryInteract scripts
            inventoryActive = false;
            mainInventoryInteract.enabled = false;
            groundInventoryInteract.enabled = false;
        }
        // If the inventory has just been opened
        if (!inventoryActive && mainInventory.gameObject.activeInHierarchy) {
            // Enable the InventoryInteract scripts
            inventoryActive = true;
            mainInventoryInteract.enabled = true;
            groundInventoryInteract.enabled = true;
        }

        if (!inventoryActive) return;

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
        // Perform right mouse button event
        if (Input.GetMouseButtonDown(1))
        {
            RightMouseButtonPress();
        }
        // Perform R key event
        if (Input.GetKeyDown(KeyCode.R))
        {
            Rotate();
        }
    }

    private void RightMouseButtonPress()
    {
        if (selectedItem == null)
        {

            Vector2 position = Input.mousePosition;

            // Find Coordinates on inventory for mouse click position
            Vector2Int tileClickPosition = selectedInventory.getTilePosition(position);

            rightClickItem = selectedInventory.getItemAtCoords(tileClickPosition.x, tileClickPosition.y);

            //If there is an item there, create a context menu for that item
            if (rightClickItem != null) contextMenuCont.createContextMenu(position, rightClickItem);
        }
    }

    // Performs place/pickup 
    private void LeftMouseButtonPress()
    {
        contextMenuCont.gameObject.SetActive(false);

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

    // Perform rotation if there is a qualifying held item
    private void Rotate()
    {
        if (selectedItem == null) return;
        if (selectedItem.sizeHeight == selectedItem.sizeWidth && fullTileSet(selectedItem)) return;
        selectedItem.Rotate();
    }

    // Check if the tile set is full (all tiles are true)
    private bool fullTileSet(InventoryItem item)
    {
        bool[,] tileSet = item.tileSet;
        for (int x = 0; x < tileSet.GetLength(0); x++)
        {
            for (int y = 0; y < tileSet.GetLength(1); y++)
            {
                if (!tileSet[x, y]) return false;
            }
        }
        return true;
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

    // Clear the ground inventory and selected item when inventory is closed
    public void CloseInventory()
    {
        if (selectedItem != null)
        {
            if (equippedItem == selectedItem) equippedItem = null;
            // Create the pickup
            SpawnPickup(selectedItem);
            Destroy(selectedItem.gameObject);
            selectedItem = null;
        }
        // For every item in the ground inventory, create a pickup
        List<InventoryItem> items = groundInventory.items;
        foreach (InventoryItem item in items)
        {
            if (equippedItem == item) equippedItem = null;
            SpawnPickup(item);
            groundInventory.CleanGrid(item);
            Destroy(item.gameObject);
        }
        // Clear the ground inventory
        groundInventory.items = new List<InventoryItem>();

        contextMenu.SetActive(false);
    }

    // Craft a Basic Gun using 5 Basic Gun Parts
    public void CraftBasicGun()
    {
        if(UseMaterials(InventoryItem.ItemName.BasicGunPart, 5))
        {
            SpawnSelectedItem(basicGunPrefab);
        }
        contextMenuCont.gameObject.SetActive(false);
    }

    // Craft a Better Gun using 5 Better Gun Parts
    public void CraftBetterGun()
    {
        if (UseMaterials(InventoryItem.ItemName.BetterGunPart, 5))
        {
            SpawnSelectedItem(betterGunPrefab);
        }
        contextMenuCont.gameObject.SetActive(false);
    }

    // Equip an item (weapon)
    public void EquipItem()
    {
        if (equippedItem != null) equippedItem.setRegularSprite();
        equippedItem = rightClickItem;
        equippedItem.setEquippedSprite();
        contextMenuCont.gameObject.SetActive(false);
    }

    // Spawn an item as the selected item
    private void SpawnSelectedItem(GameObject prefab)
    {
        selectedItem = Instantiate(prefab).GetComponent<InventoryItem>();
        selectedRectTransform = selectedItem.GetComponent<RectTransform>();
        selectedRectTransform.localScale = selectedRectTransform.localScale * canvas.scaleFactor;
        RectTransform mainInventoryRectTransform = mainInventory.gameObject.GetComponent<RectTransform>();
        selectedRectTransform.parent = mainInventoryRectTransform;
        mainInventoryRectTransform.SetAsLastSibling();
    }

    // Use a certain amount of materials in the inventory
    public bool UseMaterials(InventoryItem.ItemName itemName, int requireCount)
    {
        // Find the materials if they exist
        Queue<InventoryItem> craftItems = FindMaterials(itemName, requireCount);
        // Return false if the materials are not present
        if (craftItems == null) return false;
        int materialCount = 0;
        while (craftItems.Count > 0)
        {
            // Dequeue all the items to retain order
            InventoryItem item = craftItems.Dequeue();
            // Do math to see how much we need to take away from each item
            if (materialCount + item.itemCount > requireCount)
            {
                item.setItemCount(item.itemCount - requireCount + materialCount);
            }
            else
            {
                materialCount += item.itemCount;
                mainInventory.CleanGrid(item);
                mainInventory.items.Remove(item);
                Destroy(item.gameObject);
            }
        }
        return true;
    }

    // Go through inventory to find materials to a certain amount
    // Returns a Queue only if it finds enough materials
    public Queue<InventoryItem> FindMaterials(InventoryItem.ItemName itemName, int requiredCount)
    {
        int materialCount = 0;
        // Queue used to retain order of materials
        Queue<InventoryItem> craftItems = new Queue<InventoryItem>();
        // List of items in the main Inventory
        List<InventoryItem> mainInventoryItems = mainInventory.items;
        foreach (InventoryItem item in mainInventoryItems)
        {
            // If item is the required item type
            if (item.itemName == itemName) {
                craftItems.Enqueue(item);
                // Check if we need more materials to reach the required amount
                materialCount += item.itemCount;
                if (materialCount >= requiredCount) return craftItems;
            }
        }
        // Enough materials were not found
        return null;
    }

    // Tells the Pickup Controller which pickup to spawn
    private void SpawnPickup(InventoryItem item)
    {
        switch (item.itemName)
        {
            case InventoryItem.ItemName.BasicAmmo:
                pickupController.ActivateBasicAmmo(item.itemCount, gameObject.transform.position);
                break;
            case InventoryItem.ItemName.BetterAmmo:
                pickupController.ActivateBetterAmmo(item.itemCount, gameObject.transform.position);
                break;
            case InventoryItem.ItemName.Katana:
                pickupController.ActivateKatana(item.itemCount, gameObject.transform.position);
                break;
            case InventoryItem.ItemName.BasicGun:
                pickupController.ActivateBasicGun(item.itemCount, gameObject.transform.position);
                break;
            case InventoryItem.ItemName.BetterGun:
                pickupController.ActivateBetterGun(item.itemCount, gameObject.transform.position);
                break;
            case InventoryItem.ItemName.BasicGunPart:
                pickupController.ActivateBasicGunPart(item.itemCount, gameObject.transform.position);
                break;
            case InventoryItem.ItemName.BetterGunPart:
                pickupController.ActivateBetterGunPart(item.itemCount, gameObject.transform.position);
                break;
        }
    }
}
