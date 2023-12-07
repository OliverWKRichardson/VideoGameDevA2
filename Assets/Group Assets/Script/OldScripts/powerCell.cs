using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerCell : MonoBehaviour
{
    public GameObject explode;
    private GameObject tripod;
    float removeTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Find the tripod
        tripod = GameObject.Find("tripod");
        // Destroy the object after 2s
        Destroy(gameObject, removeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // Reduce the tripod's health
            tripod.GetComponent<triPodHealth>().reduceHealth();
            Destroy(gameObject);
        // If colliding with a box, explode
        } else if (other.gameObject.tag == "Box")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Instantiate the explosion
        GameObject explosion = Instantiate(explode, transform.position, transform.rotation);
        // Destroy the explosion 1s later (Could be done cleaner)
        Destroy(explosion, 1.0f);
        // Find all colliders in a sphere of radius 2 around the powerCell
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);
        // Checks all colliders
        foreach (Collider collider in colliders)
        {
            // If a box is within the radius of the explosion, destroy the box
            if (collider.tag == "Box")
            {
                Destroy(collider.gameObject);
            }
        }
    }
}
