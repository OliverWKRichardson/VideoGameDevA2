using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    // Tile Size constant (40 pixels per tile)
    public const float tileDimension = 40;
    [SerializeField] int gridSizeWidth = 10;
    [SerializeField] int gridSizeHeight = 5;
    Vector2 gridPosition = new Vector2();
    Vector2Int tilePosition = new Vector2Int();

    // Canvas used for scaling
    [SerializeField] Canvas canvas;
    RectTransform rectTransform;

    // 2D Array for storing inventory tile data
    public InventoryItem[,] inventoryItemSlot;

    public List<InventoryItem> items;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        InitiateSize(gridSizeWidth, gridSizeHeight);

        spawnItemsForTesting spawner = GetComponent<spawnItemsForTesting>();
        if (spawner != null) {
            spawner.spawn();
        }
    }

    // Change size of inventory based on grid size
    private void InitiateSize(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileDimension, height * tileDimension);
        rectTransform.sizeDelta = size;
    }

    // Calculate which tile the mouse is located at
    public Vector2Int getTilePosition(Vector2 mousePosition)
    {
        // Calculate transform position of mouse on the inventory
        gridPosition.x = mousePosition.x - rectTransform.position.x;
        gridPosition.y = rectTransform.position.y - mousePosition.y;

        // Use tileDimension and canvas scaleFactor to determine grid coordinates
        tilePosition.x = (int)(gridPosition.x / (tileDimension * canvas.scaleFactor));
        tilePosition.y = (int)(gridPosition.y / (tileDimension * canvas.scaleFactor));

        return tilePosition;
    }

    // Manually spawn items at posx, posy
    public bool SpawnItem(GameObject inventoryItemPrefab, int posx, int posy, int itemCount)
    {
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        RectTransform instantRectTransform = inventoryItem.GetComponent<RectTransform>();
        instantRectTransform.localScale = instantRectTransform.localScale * canvas.scaleFactor;

        if (inventoryItem.isStackable) inventoryItem.setItemCount(itemCount);

        if (!BoundaryCheck(posx, posy, inventoryItem.sizeWidth, inventoryItem.sizeHeight))
        {
            return false;
        }

        moveItem(inventoryItem, posx, posy);

        return true;
    }

    // User places picked up item at posx, posy and checks for any overlaps
    public bool PlaceItem(ref InventoryItem inventoryItem, int posx, int posy, ref InventoryItem overlapItem)
    {
        // Check that item is within the bounds of the inventory
        if (!BoundaryCheck(posx, posy, inventoryItem.sizeWidth, inventoryItem.sizeHeight))
        {
            return false;
        }

        // Check if the item overlaps multiple objects and can't be placed
        if (!OverlapCheck(posx, posy, ref inventoryItem, inventoryItem.sizeWidth, inventoryItem.sizeHeight, inventoryItem.tileSet, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        // If there is one overlap, clean references in the grid to the overlapItem
        if (overlapItem != null)
        {
            CleanGrid(overlapItem);
            items.Remove(overlapItem);
        }

        moveItem(inventoryItem, posx, posy);

        return true;
    }

    private void moveItem(InventoryItem inventoryItem, int posx, int posy)
    {
        // Set the item as a child to the Inventory object
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        bool[,] tileSet = inventoryItem.tileSet;
        // Populate grid with references to the placed item
        for (int x = 0; x < inventoryItem.sizeWidth; x++)
        {
            for (int y = 0; y < inventoryItem.sizeHeight; y++)
            {
                // Only assign item at coord if the tile set says there is a tile
                if (tileSet[x, y]) inventoryItemSlot[posx + x, posy + y] = inventoryItem;
            }
        }
        items.Add(inventoryItem);

        // Set item's onGridPosition
        inventoryItem.onGridPositionX = posx;
        inventoryItem.onGridPositionY = posy;

        // Position the item on the grid considering the size of the item
        Vector2 position = new Vector2();
        position.x = posx * tileDimension + tileDimension * inventoryItem.sizeWidth / 2;
        position.y = -(posy * tileDimension + tileDimension * inventoryItem.sizeHeight / 2);

        rectTransform.localPosition = position;
    }

    // Checks for overlapping items when placing
    // If 0 overlaps, returns true
    // If 1 overlap, returns true
    // If 2 unique item overlaps, return false
    private bool OverlapCheck(int posx, int posy, ref InventoryItem inventoryItem, int width, int height, bool[,] tileSet, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tileSet[x, y])
                {
                    if (inventoryItemSlot[posx + x, posy + y] != null)
                    {
                        if (overlapItem == null)
                        {
                            overlapItem = inventoryItemSlot[posx + x, posy + y];
                            // If items are the same type and can be stacked
                            if (inventoryItem.itemName == overlapItem.itemName && inventoryItem.isStackable)
                            {
                                // If the overlap item is full, just swap the items
                                if (overlapItem.itemCount == overlapItem.itemCountMax) return true;
                                if (overlapItem.itemCount + inventoryItem.itemCount > overlapItem.itemCountMax)
                                {
                                    int difference = overlapItem.itemCountMax - overlapItem.itemCount;
                                    overlapItem.setItemCount(overlapItem.itemCount + difference);
                                    inventoryItem.setItemCount(inventoryItem.itemCount - difference);
                                    if (inventoryItem.itemCount == 0) inventoryItem = null;
                                } else
                                {
                                    overlapItem.setItemCount(overlapItem.itemCount + inventoryItem.itemCount);
                                    inventoryItem.setItemCount(0);
                                    inventoryItem = null;
                                }
                                return false;
                            }

                        }
                        else
                        {
                            if (overlapItem != inventoryItemSlot[posx + x, posy + y])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        rectTransform.SetAsLastSibling();
        return true;
    }

    public InventoryItem getItemAtCoords(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    // Removes picked up item and returns it
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem pickedUpItem = inventoryItemSlot[x, y];

        if (pickedUpItem == null) return null;
        rectTransform.SetAsLastSibling();
        CleanGrid(pickedUpItem);
        items.Remove(pickedUpItem);

        return pickedUpItem;
    }

    // Removes references in the grid for an item
    public void CleanGrid(InventoryItem item)
    {
        bool[,] tileSet = item.tileSet;
        for (int ix = 0; ix < item.sizeWidth; ix++)
        {
            for (int iy = 0; iy < item.sizeHeight; iy++)
            {
                if (tileSet[ix, iy]) inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    // Checks if coordinate is outside of grid
    private bool PositionCheck(int posx, int posy)
    {
        if (posx < 0 || posy < 0) return false;
        if (posx >= gridSizeWidth || posy >= gridSizeHeight) return false;

        return true;
    }

    // Checks if any boundary is outside of grid
    private bool BoundaryCheck(int posx, int posy, int width, int height)
    {
        if (PositionCheck(posx, posy) == false) return false;

        posx += width-1;
        posy += height-1;

        if (PositionCheck(posx, posy) == false) return false;

        return true;
    }
}
