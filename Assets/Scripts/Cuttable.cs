using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable : MonoBehaviour
{
    private bool isCuttable;
    public GameObject splat;
    private GameObject player;

    private void Start()
    {
        isCuttable = true;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        // kill if under relativePositionOnScreen
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
            // end the streak if the item is a fruit
            if(tag == "Fruit")
            {
                player.GetComponent<Ninja_Player>().endStreak();
            }
        }
    }

    public void Hit()
    {
        if(isCuttable)
        {
            // if fruit then place splat on relativePositionOnScreen
            if(tag == "Fruit")
            {
                GameObject splatted = Instantiate(splat, gameObject.transform.position, Quaternion.Euler(0, 0, Random.Range(-180F, 180F))) as GameObject;
            }
            // remove object when cut
            Destroy(gameObject);
        }
    }
}
