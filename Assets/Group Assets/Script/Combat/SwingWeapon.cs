using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWeapon : MonoBehaviour
{
    // Animator for the model
    Animator animator;
    
    // Audio sound played on swing
    AudioSource audioSource;

    // Script for this hitbox infront of the player
    [SerializeField] MeleeHitBox hitbox;

    // Damage that the weapon does to the enemy
    [SerializeField] float damage;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Perform the attack
    public void Swing()
    {
        // Play the animation and sound
        animator.SetTrigger("KatanaSwing");
        audioSource.Play();

        // Get all the enemies infront of the player
        List<GameObject> enemies = hitbox.enemies;
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyHealth>().DamageBy(damage);
        }
    }
}
