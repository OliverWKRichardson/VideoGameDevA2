using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    // Prefabs for items, their initial count and Lists
    [SerializeField] GameObject basicAmmo;
    List<GameObject> basicAmmoList;
    [SerializeField] int ICBasicAmmo;
    [SerializeField] GameObject betterAmmo;
    List<GameObject> betterAmmoList;
    [SerializeField] int ICBetterAmmo;
    [SerializeField] GameObject katana;
    List<GameObject> katanaList;
    [SerializeField] int ICKatana;
    [SerializeField] GameObject basicGun;
    List<GameObject> basicGunList;
    [SerializeField] int ICBasicGun;
    [SerializeField] GameObject betterGun;
    List<GameObject> betterGunList;
    [SerializeField] int ICBetterGun;
    [SerializeField] GameObject basicGunPart;
    List<GameObject> basicGunPartList;
    [SerializeField] int ICBasicGunPart;
    [SerializeField] GameObject betterGunPart;
    List<GameObject> betterGunPartList;
    [SerializeField] int ICBetterGunPart;

    void Awake()
    {
        basicAmmoList = new List<GameObject>();
        betterAmmoList = new List<GameObject>();
        katanaList = new List<GameObject>();
        basicGunList = new List<GameObject>();
        betterGunList = new List<GameObject>();
        basicGunPartList = new List<GameObject>();
        betterGunPartList = new List<GameObject>();

        instantiatePrefabs(basicAmmo, basicAmmoList, ICBasicAmmo);
        instantiatePrefabs(betterAmmo, betterAmmoList, ICBetterAmmo);
        instantiatePrefabs(katana, katanaList, ICKatana);
        instantiatePrefabs(basicGun, basicGunList, ICBasicGun);
        instantiatePrefabs(betterGun, betterGunList, ICBetterGun);
        instantiatePrefabs(basicGunPart, basicGunPartList, ICBasicGunPart);
        instantiatePrefabs(betterGunPart, betterGunPartList, ICBetterGunPart);
    }

    // Populate the Lists with the relevant amount of pickups
    private void instantiatePrefabs(GameObject prefab, List<GameObject> list, int initialCount)
    {
        for (int i = 0; i < initialCount; i++)
        {;
            GameObject pickup = Instantiate(prefab);
            pickup.SetActive(false);
            pickup.transform.SetParent(this.transform);
            list.Add(pickup);
        }
    }

    // Object pooling algorithm that will instantiate new objects if limits are hit
    private void ActivateObject(int itemCount, Vector3 position, GameObject prefab, ref List<GameObject> list)
    {
        // Find an un-active pickup and move to position
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeInHierarchy)
            {
                list[i].SetActive(true);
                list[i].transform.position = position;
                list[i].GetComponent<Pickup>().itemCount = itemCount;
                return;
            }
        }
        // If there are no more free pickups, instantiate a new one
        GameObject pickup = Instantiate(prefab);
        pickup.transform.SetParent(this.transform);
        pickup.transform.position = position;
        pickup.GetComponent<Pickup>().itemCount = itemCount;
        list.Add(pickup);
    }

    // Public method to be called when disabling a pickup
    public void DisablePickup(GameObject pickup, Pickup.PickupName type)
    {
        // Determines the type of the pickup
        switch (type)
        {
            case Pickup.PickupName.BasicAmmo:
                DisablePickup(basicAmmoList, ICBasicAmmo, pickup);
                break;
            case Pickup.PickupName.BetterAmmo:
                DisablePickup(betterAmmoList, ICBetterAmmo, pickup);
                break;
            case Pickup.PickupName.Katana:
                DisablePickup(katanaList, ICKatana, pickup);
                break;
            case Pickup.PickupName.BasicGun:
                DisablePickup(basicGunList, ICBasicGun, pickup);
                break;
            case Pickup.PickupName.BetterGun:
                DisablePickup(betterGunList, ICBetterGun, pickup);
                break;
            case Pickup.PickupName.BasicGunPart:
                DisablePickup(basicGunPartList, ICBasicGunPart, pickup);
                break;
            case Pickup.PickupName.BetterGunPart:
                DisablePickup(betterGunPartList, ICBetterGunPart, pickup);
                break;
        }
    }

    // Disables pickup and alters the respective list
    private void DisablePickup(List<GameObject> list, int initialCount, GameObject pickup)
    {
        // If the list is bigger than the initial count, destroy the pickup
        if (list.Count > initialCount)
        {
            list.Remove(pickup);
            Destroy(pickup);
        } else
        // Otherwise just disable the pickup
        {
            pickup.SetActive(false);
        }
    }

    public void ActivateBasicAmmo(int itemCount, Vector3 position)
    {
        ActivateObject(itemCount, position, basicAmmo, ref basicAmmoList);
    }

    public void ActivateBetterAmmo(int itemCount, Vector3 position)
    {
        ActivateObject(itemCount, position, betterAmmo, ref betterAmmoList);
    }

    public void ActivateKatana(int itemCount, Vector3 position)
    {
        ActivateObject(itemCount, position, katana, ref katanaList);
    }

    public void ActivateBasicGun(int itemCount, Vector3 position)
    {
        ActivateObject(itemCount, position, basicGun, ref basicGunList);
    }

    public void ActivateBetterGun(int itemCount, Vector3 position)
    {
        ActivateObject(itemCount, position, betterGun, ref betterGunList);
    }

    public void ActivateBasicGunPart(int itemCount, Vector3 position)
    {
        ActivateObject(itemCount, position, basicGunPart, ref basicGunPartList);
    }

    public void ActivateBetterGunPart(int itemCount, Vector3 position)
    {
        ActivateObject(itemCount, position, betterGunPart, ref betterGunPartList);
    }

}
