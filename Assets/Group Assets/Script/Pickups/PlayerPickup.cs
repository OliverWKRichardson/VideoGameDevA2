using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    // Player script
    [SerializeField] FirstPersonAIO player;
    // Distance from player that items can be picked up
    [SerializeField] float pickupDistance;

    // Text Mesh Pro which tells user to pickup items
    [SerializeField] TextMeshProUGUI pickupMessage;

    // Inventory in which pickups are turned to items
    [SerializeField] Inventory groundInventory;

    // Manages deletion of pickups
    [SerializeField] PickupController pickupController;

    // Prefabs for all items
    [SerializeField] GameObject basicAmmo;
    [SerializeField] GameObject betterAmmo;
    [SerializeField] GameObject katana;
    [SerializeField] GameObject basicGun;
    [SerializeField] GameObject betterGun;
    [SerializeField] GameObject basicGunPart;
    [SerializeField] GameObject betterGunPart;

    // Current pickup being looked at
    Pickup currentPickup;

    void Update()
    {
        // Check for pickup
        Pickup newPickup = GetViewedPickup();

        // If new pickup
        if (newPickup != currentPickup)
        {
            currentPickup = newPickup;
            if (currentPickup != null)
            {
                // Enable text
                pickupMessage.gameObject.SetActive(true);
                // Tell user to pickup the item
                if (currentPickup.itemCount == 1) {
                    pickupMessage.SetText("Press f to pickup " + currentPickup.displayName);
                } else {
                    pickupMessage.SetText("Press f to pickup " + currentPickup.displayName + " (" + currentPickup.itemCount + ")");
                }
            } else
            {
                // Hide the text
                pickupMessage.gameObject.SetActive(false);
            }
        }

        // When F is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            // If there is no pickup or inventory is open then stop
            if (currentPickup == null || groundInventory.gameObject.activeInHierarchy) return;

            // Turn on inventory
            player.ToggleInventory();
            // Spawn respective item in the ground inventory
            switch (currentPickup.pickupName)
            {
                case Pickup.PickupName.BasicAmmo:
                    groundInventory.SpawnItem(basicAmmo, 0, 0, currentPickup.itemCount);
                    break;
                case Pickup.PickupName.BetterAmmo:
                    groundInventory.SpawnItem(betterAmmo, 0, 0, currentPickup.itemCount);
                    break;
                case Pickup.PickupName.Katana:
                    groundInventory.SpawnItem(katana, 0, 0, currentPickup.itemCount);
                    break;
                case Pickup.PickupName.BasicGun:
                    groundInventory.SpawnItem(basicGun, 0, 0, currentPickup.itemCount);
                    break;
                case Pickup.PickupName.BetterGun:
                    groundInventory.SpawnItem(betterGun, 0, 0, currentPickup.itemCount);
                    break;
                case Pickup.PickupName.BasicGunPart:
                    groundInventory.SpawnItem(basicGunPart, 0, 0, currentPickup.itemCount);
                    break;
                case Pickup.PickupName.BetterGunPart:
                    groundInventory.SpawnItem(betterGunPart, 0, 0, currentPickup.itemCount);
                    break;
            }
            // Either disables the pickup or destroys it based on object pooling
            pickupController.DisablePickup(currentPickup.gameObject, currentPickup.pickupName);
            currentPickup = null;
            pickupMessage.gameObject.SetActive(false);
        }
    }

    private Pickup GetViewedPickup()
    {
        RaycastHit[] hits;
        // Get all objects infront of player within pickupDistance
        hits = Physics.RaycastAll(transform.position, transform.forward, pickupDistance);
        // Array is ordered backwards by distance
        for (int i = hits.Length - 1; i >= 0; i--)
        {
            // If object is Player, ignore
            if (!hits[i].transform.gameObject.CompareTag("Player"))
            {
                // If pickup, return
                if (hits[i].transform.gameObject.CompareTag("Pickup"))
                {
                    return hits[i].transform.gameObject.GetComponent<Pickup>();
                }
                else
                {
                    break;
                }
            }
        }
        return null;
    }
}
