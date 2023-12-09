using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] Canvas dialogueCanvas;

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

    // Current npc being looked at
    NPCDialogue currentNPC;

    void Update()
    {
        // Check for pickup
        GameObject newGameObject = GetViewedGameobject();

        if (newGameObject == null || dialogueCanvas.gameObject.activeInHierarchy) {
            currentPickup = null;
            currentNPC = null;
            pickupMessage.gameObject.SetActive(false);
            return;
        }

        Pickup newPickup = newGameObject.GetComponent<Pickup>();
        NPCDialogue newNPC = newGameObject.GetComponent<NPCDialogue>();

        
        // If new pickup
        if (newPickup != currentPickup)
        {
            currentPickup = newPickup;
            if (currentPickup != null)
            {
                // Enable text
                pickupMessage.gameObject.SetActive(true);
                // Tell user to pickup the item
                if (currentPickup.itemCount == 1)
                {
                    pickupMessage.SetText("Press f to pickup " + currentPickup.displayName);
                }
                else
                {
                    pickupMessage.SetText("Press f to pickup " + currentPickup.displayName + " (" + currentPickup.itemCount + ")");
                }
            }
        }
        
        // If new npc
        if (newNPC != currentNPC)
        {
            currentNPC = newNPC;
            if (currentNPC != null)
            {
                // Enable text
                pickupMessage.gameObject.SetActive(true);
                // Tell user to talk

                pickupMessage.SetText("Press f to talk to " + currentNPC.NpcName);
            }
        }

        // When F is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (groundInventory.gameObject.activeInHierarchy || dialogueCanvas.gameObject.activeInHierarchy) return;

            if (currentPickup != null)
            {
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
            } else if (currentNPC != null)
            {
                currentNPC.startDialogue();
            }
        }
    }

    private GameObject GetViewedGameobject()
    {
        RaycastHit[] hits;
        // Get all objects infront of player within pickupDistance
        hits = Physics.RaycastAll(transform.position, transform.forward, pickupDistance);
        // Order the array
        RaycastHit[] sortedHits = hits.OrderBy(hit => hit.distance).ToArray();
        // Array is ordered acending by distance
        for (int i = 0; i < sortedHits.Length; i++)
        {
            // If object is Player, ignore
            if (!sortedHits[i].transform.gameObject.CompareTag("Player") && !sortedHits[i].transform.gameObject.CompareTag("PlayerAttachment"))
            {
                // If pickup, return
                if (sortedHits[i].transform.gameObject.CompareTag("Pickup") || sortedHits[i].transform.gameObject.CompareTag("NPC"))
                {
                    return sortedHits[i].transform.gameObject;
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
