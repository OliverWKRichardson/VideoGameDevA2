using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    // Pickup Controller to spawn pickups
    PickupController pickupController;

    // Types of loot
    public enum lootType
    {
        basicAmmo,
        betterAmmo,
        basicGunPart,
        betterGunPart
    }

    // Loots that the enemy drops
    [SerializeField] lootType[] loots;
    // Min amount for each loot
    [SerializeField] int[] minItemCount;
    // Max amount for each loot
    [SerializeField] int[] maxItemCount;

    void Awake()
    {
        pickupController = GameObject.Find("PickupController").GetComponent<PickupController>();
    }

    // Spawn loot
    public void Loot()
    {
        for (int i = 0; i < loots.Length; i++)
        {
            switch(loots[i])
            {
                case lootType.basicAmmo:
                    pickupController.ActivateBasicAmmo(Random.Range(minItemCount[i], maxItemCount[i]), gameObject.transform.position);
                    break;
                case lootType.betterAmmo:
                    pickupController.ActivateBetterAmmo(Random.Range(minItemCount[i], maxItemCount[i]), gameObject.transform.position);
                    break;
                case lootType.basicGunPart:
                    pickupController.ActivateBasicGunPart(Random.Range(minItemCount[i], maxItemCount[i]), gameObject.transform.position);
                    break;
                case lootType.betterGunPart:
                    pickupController.ActivateBetterGunPart(Random.Range(minItemCount[i], maxItemCount[i]), gameObject.transform.position);
                    break;
            }
        }
    }
}
