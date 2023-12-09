using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootGun : MonoBehaviour
{
    Transform cameraTransform;

    Animator animator;

    AudioSource audioSource;

    [SerializeField] AudioClip gunshot;

    [SerializeField] ParticleSystem muzzleFlash;

    [SerializeField] float damage;

    [SerializeField] float shootDistance;

    [SerializeField] InventoryController inventoryController;

    public enum ammoType
    {
        basicAmmo,
        betterAmmo
    }

    [SerializeField] ammoType ammo;

    void Awake()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        if (ammo == ammoType.basicAmmo)
        {
            if (!inventoryController.UseMaterials(InventoryItem.ItemName.BasicAmmo, 1)) {
                return;
            }
        } else if (ammo == ammoType.betterAmmo)
        {
            if (!inventoryController.UseMaterials(InventoryItem.ItemName.BetterAmmo, 1)) {
                return;
            }
        }

        muzzleFlash.Play();
        animator.SetTrigger("Shoot");
        audioSource.PlayOneShot(gunshot, audioSource.volume);

        RaycastHit[] hits;
        // Get all objects infront of player within pickupDistance
        hits = Physics.RaycastAll(cameraTransform.position, cameraTransform.forward, shootDistance);
        // Order the array
        RaycastHit[] sortedHits = hits.OrderBy(hit => hit.distance).ToArray();
        // Array is ordered ascending by distance
        for (int i = 0; i < sortedHits.Length; i++)
        {
            // If object is Player, ignore
            if (!sortedHits[i].transform.gameObject.CompareTag("Player") && !sortedHits[i].transform.gameObject.CompareTag("PlayerAttachment"))
            {
                // If Enemy, hit
                if (sortedHits[i].transform.gameObject.CompareTag("Enemy"))
                {
                    // TODO hit enemy for damage
                    Debug.Log(sortedHits[i].transform.gameObject.name);
                }
                else
                {
                    // TODO particles for hitting wall
                }
                break;
            }
        }
    }
}
