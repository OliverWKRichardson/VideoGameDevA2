using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    // Rotation of item
    public float rotation = 0;

    // General size of item
    public int sizeWidth = 1;
    public int sizeHeight = 1;

    // Coordinates of item in Inventory
    [HideInInspector] public int onGridPositionX;
    [HideInInspector] public int onGridPositionY;

    // 1D Array representation of which tiles are present
    // Can be processed into 2D Array
    [SerializeField] bool[] tileArray;

    public bool[,] tileSet;

    void Awake()
    {
        // Alter the size of the object based on tileDimension
        Vector2 size = new Vector2();
        size.x = sizeWidth * Inventory.tileDimension;
        size.y = sizeHeight * Inventory.tileDimension;

        GetComponent<RectTransform>().sizeDelta = size;

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
        int tempValue = sizeWidth;
        sizeWidth = sizeHeight;
        sizeHeight = tempValue;
    }

    private void TileSetRotate()
    {

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

    private bool[,] ReverseRow(bool[,] tileSet)
    {
        for (int y = 0; y < tileSet.GetLength(1); y++)
        {
            int start = 0;
            int end = tileSet.GetLength(1) - 1;
            while(start < end)
            {

            }
        }
        return tileSet;
    }
}
