using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Items : MonoBehaviour
{
    public float spawnTime = 1;
    public GameObject apple;
    public GameObject bomb;
    public float upForce = 750;
    public float leftRightForce = 200;
    public float maxX = -7;
    public float minX = 7;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnTime);
        GameObject prefab = apple;
        if (Random.Range(0, 100) < 30)
        {
            prefab = bomb;
        }
        GameObject go = Instantiate(prefab, new Vector3(Random.Range(minX, maxX + 1), transform.position.y, 0f), Quaternion.Euler(0, 0, Random.Range(-90F, 90F))) as GameObject;
        if (go.transform.position.x > 0)
        {
            go.GetComponent<Rigidbody2D>().AddForce(new Vector2(-leftRightForce, upForce));
        }
        else
        {
            go.GetComponent<Rigidbody2D>().AddForce(new Vector2(leftRightForce, upForce));
        }
        StartCoroutine("Spawn");
    }
}
