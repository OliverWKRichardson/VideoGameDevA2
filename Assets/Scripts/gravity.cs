using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity : MonoBehaviour
{
    private Vector3 acceleration;
    private Vector3 gravforce;
    private Vector3 velocity;
    private float mass = 10.0f;

    public void applyForce(Vector3 force)
    {
        Vector3 a = force / mass;
        acceleration += a;
    }

    private void updatePos()
    {
        velocity = velocity + acceleration;
        transform.position += velocity * Time.deltaTime;
        acceleration = new Vector3(0.0f, 0.0f); //reset to zero
    }

    // Start is called before the first frame update
    void Start()
    {
        gravforce = new Vector3(0, -1, 0);
    }

    void LateUpdate()
    {
        applyForce(gravforce);
        updatePos();
    }
}
