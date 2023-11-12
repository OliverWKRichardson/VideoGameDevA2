using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    private Vector3 acceleration;
    private Vector3 velocity;
    public int mass;

    public void applyForce(Vector3 force)
    {
        Vector3 a = force / mass;
        acceleration += a;
    }

    private void updatePos()
    {
        velocity += acceleration;
        transform.position += velocity * Time.deltaTime;
        acceleration = new Vector3(0.0f, 0.0f); //reset to zero
    }

    void FixedUpdate()
    {
        // gravity
        applyForce(new Vector3(0, -1, 0));
    }

    void LateUpdate()
    {
        // update position of object
        updatePos();
    }
}
