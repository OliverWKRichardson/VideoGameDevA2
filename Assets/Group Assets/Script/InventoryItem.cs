using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public int itemCount;
    public int itemCountMax;

    public bool isStackable;

    public enum ItemName
    {
        BasicAmmo,
        BetterAmmo,
        Dagger,
        BasicGun,
        BetterGun,
        BasicGunPart,
        BetterGunPart
    }

    public ItemName itemName;

    [SerializeField] TextMeshProUGUI itemCountText;

    void Awake()
    {
        // Alter the size of the object based on tileDimension
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

    public void Rotate()
    {
        // Rotate the 2D array
        TileSetRotate();
        // Rotate the item and switch width and height
        rotation += 90;
        if (rotation == 360) rotation = 0;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, -rotation);
        RectTransform textRectTransform = GetComponent<RectTransform>();
        textRectTransform.rotation = Quaternion.Euler(0, 0, rotation);
        int tempValue = sizeWidth;
        sizeWidth = sizeHeight;
        sizeHeight = tempValue;
    }

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

    private void TileSetRotate()
    {
        tileSet = Transpose(ReverseColumn(tileSet));
    }

    private bool[,] Transpose(bool[,] tileSet)
    {
        bool[,] newTileSet = new bool[tileSet.GetLength(1), tileSet.GetLength(0)];
        for (int x = 0; x < tileSet.GetLength(0); x++)
        {
            for (int y = 0; y < tileSet.GetLength(1); y++)
            {
                newTileSet[y, x] = tileSet[x, y];
            }
        }
        return newTileSet;
    }

    private bool[,] ReverseColumn(bool[,] tileSet)
    {
        for (int x = 0; x < tileSet.GetLength(0); x++)
        {
            int start = 0;
            int end = tileSet.GetLength(1) - 1;
            while(start < end)
            {
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
