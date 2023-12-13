using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootGun : MonoBehaviour
{
    // Transform to shoot the ray from
    Transform cameraTransform;

    // Animator for the model
    Animator animator;

    // Audio sound played on shot
    AudioSource audioSource;

    // Audio clip (used to allow audio stacking)
    [SerializeField] AudioClip gunshot;

    // Particle system that makes the muzzle flash
    [SerializeField] ParticleSystem muzzleFlash;

    // Damage the shot does
    [SerializeField] float damage;

    // Distance the bullet will go
    [SerializeField] float shootDistance;

    // Distance the sound will go
    [SerializeField] float soundDistance;

    // Inventory controller used to use ammo
    [SerializeField] InventoryController inventoryController;

    // Type of ammo used by the gun
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

    // Shoot the weapon
    public void Shoot()
    {
        // Check if the player has ammo, if not, dont shoot
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

        // Animate the gun and play sound
        muzzleFlash.Play();
        animator.SetTrigger("Shoot");
        // PlayOneShot used so that gun shots stack
        audioSource.PlayOneShot(gunshot, audioSource.volume);

        // Make a noise
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, soundDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Enemy")
            {
                hitCollider.gameObject.GetComponent<Pathing>().HeardNoise(transform.position);
            }
        }

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
                    sortedHits[i].transform.gameObject.GetComponent<EnemyHealth>().DamageBy(damage);
                    sortedHits[i].transform.gameObject.GetComponent<Pathing>().HeardNoise(transform.position);
                }
                break;
            }
        }
    }
}
