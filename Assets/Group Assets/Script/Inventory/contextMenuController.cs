using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class contextMenuController : MonoBehaviour
{
    // RectTransform for the context menu
    RectTransform rectTransform;

    // The elements of the context menu
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Button equipButton;
    [SerializeField] Button craftBasicGunButton;
    [SerializeField] Button craftBetterGunButton;

    // Rect Transform for each button
    RectTransform equipButtonRT;
    RectTransform craftBasicGunRT;
    RectTransform craftBetterGunRT;

    void Awake()
    {
        // Hide the context menu and clear it
        gameObject.SetActive(false);
        clearContextMenu();
        rectTransform = GetComponent<RectTransform>();
        equipButtonRT = equipButton.GetComponent<RectTransform>();
        craftBasicGunRT = craftBasicGunButton.GetComponent<RectTransform>();
        craftBetterGunRT = craftBetterGunButton.GetComponent<RectTransform>();
    }

    // Clear the context menu by hiding the buttons
    private void clearContextMenu()
    {
        gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        craftBasicGunButton.gameObject.SetActive(false);
        craftBetterGunButton.gameObject.SetActive(false);
    }

    // Move to a position on the canvas
    private void moveToLocation(Vector2 position)
    {
        gameObject.SetActive(true);
        rectTransform.position = position;
    }

    // Create context menu based on the item
    public void createContextMenu(Vector2 position, InventoryItem item)
    {
        // Display the item name
        itemNameText.SetText(item.displayName);
        clearContextMenu();
        // Initial y level which changes if a button is moved
        int y = -20;
        // Get the options the item should have
        InventoryItem.contextOptions[] options = item.contextMenuList;
        // If Equipable, give the equip button etc.
        if (contains(options, InventoryItem.contextOptions.Equipable)) {
            equipButton.gameObject.SetActive(true);
            equipButtonRT.localPosition = new Vector3(0, y, 0);
            y -= 20;
        }
        if (contains(options, InventoryItem.contextOptions.BasicGunCraft))
        {
            craftBasicGunButton.gameObject.SetActive(true);
            craftBasicGunRT.localPosition = new Vector3(0, y, 0);
            y -= 20;
        }
        if (contains(options, InventoryItem.contextOptions.BetterGunCraft))
        {
            craftBetterGunButton.gameObject.SetActive(true);
            craftBetterGunRT.localPosition = new Vector3(0, y, 0);
            y -= 20;
        }
        // Change the size of the panel based on the number of buttons
        rectTransform.sizeDelta = new Vector2(160, -y);
        // Move to the mouse location
        moveToLocation(position);
    }

    // Check if an array contains an object
    private bool contains(Array array, object obj)
    {
        return Array.IndexOf(array, obj) != -1;
    }
}
