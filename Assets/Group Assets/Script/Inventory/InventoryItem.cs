using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    // Rotation of item
    private float rotation = 0;

    // General size of item
    public int sizeWidth = 1;
    public int sizeHeight = 1;

    // Coordinates of item in Inventory
    [HideInInspector] public int onGridPositionX;
    [HideInInspector] public int onGridPositionY;

    // 1D Array representation of which tiles are present
    // Can be processed into 2D Array
    // Array of 1 0 1 0 1 1 and item width of 2 becomes:
    // 1 0
    // 1 0
    // 1 1
    [SerializeField] bool[] tileArray;
    public bool[,] tileSet;

    // Amount of item
    public int itemCount;
    // Max amount of item
    public int itemCountMax;

    // Is this a stackable item
    public bool isStackable;

    // Which item this is
    public enum ItemName
    {
        BasicAmmo,
        BetterAmmo,
        Katana,
        BasicGun,
        BetterGun,
        BasicGunPart,
        BetterGunPart
    }

    public ItemName itemName;

    // Display name for this item
    public string displayName;

    // Which options should be displayed in the context menu for this item
    public enum contextOptions
    {
        Equipable,
        BasicGunCraft,
        BetterGunCraft
    }

    public contextOptions[] contextMenuList;

    [SerializeField] TextMeshProUGUI itemCountText;

    // Sprite for this item
    [SerializeField] Sprite regularSprite;
    // Sprite for this item if it can be equipped (not necessary on all items)
    [SerializeField] Sprite equippedSprite;
    // Image component used to change sprites
    Image imageComponent;

    void Awake()
    {
        imageComponent = GetComponent<Image>();

        // Alter the size of the item based on tileDimension
        Vector2 size = new Vector2();
        size.x = sizeWidth * Inventory.tileDimension;
        size.y = sizeHeight * Inventory.tileDimension;

        GetComponent<RectTransform>().sizeDelta = size;

        // Turn 1D array into 2D array
        tileSet = new bool[sizeWidth, sizeHeight];
        for (int x = 0; x < sizeWidth; x++)
        {
            for (int y = 0; y < sizeHeight; y++)
            {
                tileSet[x, y] = tileArray[x + y * sizeWidth];
            }
        }
    }

    // Rotate the item
    public void Rotate()
    {
        // Rotate the 2D array
        TileSetRotate();
        // Rotate the item
        rotation += 90;
        if (rotation == 360) rotation = 0;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, -rotation);
        // Reset the rotation of the text display if present
        if (isStackable)
        {
            RectTransform textRectTransform = itemCountText.GetComponent<RectTransform>();
            textRectTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // Swap the width and height
        int tempValue = sizeWidth;
        sizeWidth = sizeHeight;
        sizeHeight = tempValue;
    }

    // Set the item count and update the item text display
    public void setItemCount(int itemCount)
    {
        if (!isStackable) return;
        if (itemCount == 0)
        {
            Destroy(gameObject);
            return;
        }
        this.itemCount = itemCount;
        itemCountText.text = itemCount.ToString();
    }

    // Used for setting the regular sprite
    public void setRegularSprite()
    {
        imageComponent.sprite = regularSprite;
    }

    // Used for setting the sprite to equipped (if this item can be equipped)
    public void setEquippedSprite()
    {
        if (equippedSprite != null)
        {
            imageComponent.sprite = equippedSprite;
        }
    }

    // Rotate the tile set by reversing each column, then transposing
    private void TileSetRotate()
    {
        tileSet = Transpose(ReverseColumn(tileSet));
    }

    // Transpose a 2D array
    private bool[,] Transpose(bool[,] tileSet)
    {
        bool[,] newTileSet = new bool[tileSet.GetLength(1), tileSet.GetLength(0)];
        for (int x = 0; x < tileSet.GetLength(0); x++)
        {
            for (int y = 0; y < tileSet.GetLength(1); y++)
            {
                // Move the obect at x, y to y, x
                newTileSet[y, x] = tileSet[x, y];
            }
        }
        return newTileSet;
    }

    // Reverse columns of a 2D array
    private bool[,] ReverseColumn(bool[,] tileSet)
    {
        for (int x = 0; x < tileSet.GetLength(0); x++)
        {
            // start and end indexes
            int start = 0;
            int end = tileSet.GetLength(1) - 1;
            while(start < end)
            {
                // Go along the column from each end and swap
                bool temp = tileSet[x, start];
                tileSet[x, start] = tileSet[x, end];
                tileSet[x, end] = temp;
                start++;
                end--;
            }
        }
        return tileSet;
    }
}
