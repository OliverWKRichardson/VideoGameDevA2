using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    // List to store Enemy GameObjects in front of the player
    [HideInInspector] public List<GameObject> enemies = new List<GameObject>();

    // When an object enters the hitbox
    private void OnTriggerEnter(Collider other)
    {
        // If enemy add to the list
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject);
        }
    }

    // When an object leaves the hitbox
    private void OnTriggerExit(Collider other)
    {
        // If enemy remove from the list
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
        }
    }
}
