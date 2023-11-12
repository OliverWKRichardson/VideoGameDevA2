using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public float spawnTime;
    public GameObject applePrefab;
    public GameObject bombPrefab;
    private float upForce;
    private float leftRightForce;
    private float maxHorizontalSpawn;
    private float minHorizontalSpawn;
    private enum objectType{bomb, apple}
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        // wait for spawn delay
        yield return new WaitForSeconds(spawnTime);
        // randomly pick object to spawn
        objectType objectToSpawn;
        if (Random.Range(0, 100) < 30)
        {
            objectToSpawn = objectType.bomb;
        }
        else
        {
            objectToSpawn = objectType.apple;
        }
        // get spawn condition for object being spawned
        GameObject prefab = applePrefab;
        switch(objectToSpawn)
        {
            case objectType.apple:
                prefab = applePrefab;
                upForce = 100;
                leftRightForce = 30;
                maxHorizontalSpawn = -7;
                minHorizontalSpawn = 7;
                break;
            case objectType.bomb:
                prefab = bombPrefab;
                upForce = 100;
                leftRightForce = 30;
                maxHorizontalSpawn = -7;
                minHorizontalSpawn = 7;
                break;
        }
        // spawn object
        GameObject go = Instantiate(prefab, new Vector3(Random.Range(minHorizontalSpawn, maxHorizontalSpawn + 1), transform.position.y, 0f), Quaternion.Euler(0, 0, Random.Range(-90F, 90F))) as GameObject;
        if (go.transform.position.x > 0)
        {
            go.GetComponent<CustomPhysics>().applyForce(new Vector2(-leftRightForce, upForce));
        }
        else
        {
            go.GetComponent<CustomPhysics>().applyForce(new Vector2(leftRightForce, upForce));
        }
        
        // loop spawning
        StartCoroutine("Spawn");
    }
}
