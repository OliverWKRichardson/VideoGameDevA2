using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectible : MonoBehaviour
{
    public GameObject shooter;
    public AudioClip cellPickUp;

    // Upon collision between this object and another
    void OnTriggerEnter(Collider other)
    {
        // Triggers only when the other object has the Player tag
        if (other.gameObject.CompareTag("Player"))
        {
            // Sets the collectible to unactive to stop it being collected again
            gameObject.SetActive(false);
            // Plays the specified clip at the location of the collectible
            AudioSource.PlayClipAtPoint(cellPickUp, transform.position);
            // Increments the no_cell value in the shooter script.
            shooter.GetComponent<shooter>().no_cell++;
        }
    }

    void Update()
    {
        // Rotates the collectibile on the Y axis every update
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }
}
