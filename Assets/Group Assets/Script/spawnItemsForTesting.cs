using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnItemsForTesting : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    [SerializeField] GameObject basicAmmo;
    [SerializeField] GameObject betterAmmo;
    [SerializeField] GameObject katana;
    [SerializeField] GameObject basicGun;
    [SerializeField] GameObject betterGun;
    [SerializeField] GameObject basicGunPart;
    [SerializeField] GameObject betterGunPart;

    public void spawn()
    {
        inventory.SpawnItem(basicAmmo, 0, 0, 60);
        inventory.SpawnItem(basicAmmo, 1, 0, 67);
        inventory.SpawnItem(betterAmmo, 0, 1, 30);
        inventory.SpawnItem(betterAmmo, 0, 2, 28);
        inventory.SpawnItem(katana, 2, 0, 1);
        inventory.SpawnItem(basicGun, 5, 0, 1);
        inventory.SpawnItem(betterGun, 0, 3, 1);
        inventory.SpawnItem(basicGunPart, 2, 1, 6);
        inventory.SpawnItem(basicGunPart, 4, 1, 7);
        inventory.SpawnItem(betterGunPart, 4, 3, 5);
        inventory.SpawnItem(betterGunPart, 7, 3, 5);
    }
}
