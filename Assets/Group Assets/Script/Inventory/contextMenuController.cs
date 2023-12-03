using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class contextMenuController : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] Button equipButton;
    [SerializeField] Button craftBasicGunButton;
    [SerializeField] Button craftBetterGunButton;

    RectTransform equipButtonRT;
    RectTransform craftBasicGunRT;
    RectTransform craftBetterGunRT;

    void Awake()
    {
        gameObject.SetActive(false);
        clearContextMenu();
        rectTransform = GetComponent<RectTransform>();
        equipButtonRT = equipButton.GetComponent<RectTransform>();
        craftBasicGunRT = craftBasicGunButton.GetComponent<RectTransform>();
        craftBetterGunRT = craftBetterGunButton.GetComponent<RectTransform>();
    }

    private void clearContextMenu()
    {
        gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        craftBasicGunButton.gameObject.SetActive(false);
        craftBetterGunButton.gameObject.SetActive(false);
    }

    private void moveToLocation(Vector2 position)
    {
        gameObject.SetActive(true);
        rectTransform.position = position;
    }

    public void createContextMenu(Vector2 position, InventoryItem item)
    {
        clearContextMenu();
        int y = 0;
        InventoryItem.contextOptions[] options = item.contextMenuList;
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
        if (y != 0) {
            rectTransform.sizeDelta = new Vector2(120, -y);
            moveToLocation(position);
        }
    }

    private bool contains(Array array, object obj)
    {
        return Array.IndexOf(array, obj) != -1;
    }
}
