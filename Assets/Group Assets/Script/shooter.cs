using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter : MonoBehaviour
{
    public GameObject powercell;
    public int no_cell = 1;
    public AudioClip throwSound;
    private float throwSpeed = 20;

    // Update is called once per frame
    void Update()
    {
        // If fire1 pressed, and we still have at least 1 cell
        if (Input.GetButtonDown("Fire1") && no_cell > 0)
        {
            no_cell--; // Reduce the cel
            // Play throw sound
            AudioSource.PlayClipAtPoint(throwSound, transform.position);
            // Instantiate the power cell as a game object
            GameObject cell = Instantiate(powercell, transform.position, transform.rotation) as GameObject;
            // Ask the physics engine to ignore collision between
            // power cell and our FPSController
            Physics.IgnoreCollision(transform.root.GetComponent<Collider>(), cell.GetComponent<Collider>(), true);
            // Give the powerCell a velocity so that it moves forward
            cell.GetComponent<Rigidbody>().velocity = transform.forward * throwSpeed;
        }
    }
}
