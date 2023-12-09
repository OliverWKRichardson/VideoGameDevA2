using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInterval : MonoBehaviour
{
    // Interval between weapon use in seconds
    [SerializeField] float weaponInterval;

    // Incoming weapon state determined by FirstPlayerAIO
    [HideInInspector] public bool weaponEnabled = false;
    // Current weapon state
    bool currentWeaponState = false;

    // Turns on for a cooldown between left clicks
    bool weaponCooldown = false;

    // Type of weapon
    public enum weaponType
    {
        Gun,
        Melee
    }

    [SerializeField] private weaponType type;

    ShootGun shootGun;

    SwingWeapon swingWeapon;

    void Awake()
    {
        if (type == weaponType.Gun) shootGun = GetComponent<ShootGun>();
        else swingWeapon = GetComponent<SwingWeapon>();
    }

    void Update()
    {
        // If told to fight and not fighting
        if (!currentWeaponState && weaponEnabled && !weaponCooldown) {
            currentWeaponState = true;
            // Start the fighting loop
            StartCoroutine(weaponLoop());
        }
        // If told to stop fighting and fighting
        if (currentWeaponState && !weaponEnabled) {
            currentWeaponState = false;
            // Start the cooldown
            StartCoroutine(cooldown());
        }
    }

    private IEnumerator weaponLoop()
    {
        // Loop while meant to be fighting
        while (currentWeaponState)
        {
            if (type == weaponType.Gun) shootGun.Shoot();
            else swingWeapon.Swing();
            // Wait for the fight interval
            yield return new WaitForSeconds(weaponInterval);
        }
    }

    private IEnumerator cooldown()
    {
        // Start cooldown
        weaponCooldown = true;
        // Cooldown is the length of the weapon interval
        // This is to stop spamming the left click to bypass the interval
        yield return new WaitForSeconds(weaponInterval);
        weaponCooldown = false;
    }
}
