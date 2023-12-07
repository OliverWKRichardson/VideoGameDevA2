using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int itemCount;

    // Used to show the range of values the pickup can give
    public int itemCountMax;
    public int itemCountMin;

    // Pickup type
    public enum PickupName
    {
        BasicAmmo,
        BetterAmmo,
        Katana,
        BasicGun,
        BetterGun,
        BasicGunPart,
        BetterGunPart
    }

    public PickupName pickupName;

    // Name to be displayed in the UI
    public string displayName;
}
