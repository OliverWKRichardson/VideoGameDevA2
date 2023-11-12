using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 facing;
    private Vector3 targetDirection;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        facing = this.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        targetDirection = player.transform.position - this.transform.position;
        float angle = Vector2.Angle(facing, targetDirection);
        Vector3 crossP = Vector3.Cross(facing, targetDirection);
        int clockwise = 1;
        if (crossP.z < 0)
        {
            clockwise = -1;
        }
        this.transform.rotation = Quaternion.Euler(0, 0, angle * clockwise);
    }

    private void LateUpdate()
    {
        transform.position += this.transform.up * Time.deltaTime;
    }
}
