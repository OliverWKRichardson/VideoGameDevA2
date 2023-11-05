using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit2D : MonoBehaviour
{
    private bool canBeDead;
    private Vector3 screen;
    public GameObject splat;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        screen = Camera.main.WorldToScreenPoint(transform.position);
        if (canBeDead && screen.y < -20)
        {
            Destroy(gameObject);
            if(tag == "Fruit")
            {
                player.GetComponent<Ninja_Player>().endStreak();
            }
        }
        else if (!canBeDead && screen.y > -10)
        {
            canBeDead = true;
        }
    }

    public void Hit(string what)
    {
        if(what.Equals("Fruit"))
        {
            GameObject splatted = Instantiate(splat, gameObject.transform.position, Quaternion.Euler(0, 0, Random.Range(-180F, 180F))) as GameObject;
        }
        Destroy(gameObject);
    }
}
