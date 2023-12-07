using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogRotate : MonoBehaviour
{
    Transform trans;

    void Awake()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        trans.rotation = Quaternion.Euler(-90, 0, 0);
    }
}
