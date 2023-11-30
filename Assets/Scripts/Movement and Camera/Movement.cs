using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Movement Variables
    public float movementSpeed;
    public float jumpStrength;

    // for references
    private Rigidbody playerRigidbody;
    
    private Transform mainCameraTransform;

    // for jumping
    private bool grounded;

    // for camera turn smoothing
    private float turnSmoothVelocity = 0.0f;
    private float turnSmoothTime = 0.05f;

    // for stealth
    private float noiseModifier;
    private bool crouched;

    void Start()
    {
        mainCameraTransform = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform;
        playerRigidbody = GetComponent<Rigidbody>();
        
        grounded = true;
        crouched = false;

        jumpStrength = 300.0f;
        movementSpeed = 5.0f;

        noiseModifier = 1.0f;
    }

    void Update()
    {
        // Jumping
        if (grounded && !crouched)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRigidbody.AddForce(transform.up * jumpStrength);
                grounded = false;
                // sound for stealth - jumping sound
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, (15.0f * noiseModifier));
                foreach (var hitCollider in hitColliders)
                {
                    if(hitCollider.gameObject.tag == "Enemy")
                    {
                        hitCollider.gameObject.GetComponent<Pathing>().HeardNoise(transform.position);
                    }
                }
            }
        }

        // crouching
        if (grounded)
        {
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                if(!crouched)
                {
                    crouched = true;
                    noiseModifier = 0.0f;
                    movementSpeed = 2.5f;
                }
                else
                {
                    crouched = false;
                    noiseModifier = 1.0f;
                    movementSpeed = 5.0f;
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Movement
        if (grounded) // while on the ground
        {
            // get input
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 inputVect = new Vector3(h, 0, v).normalized;

            if (inputVect.magnitude != 0.0f)
            {
                // rotate model to face direction of movement relative to camera
                float targetAngle = Mathf.Atan2(inputVect.x, inputVect.z) * Mathf.Rad2Deg + mainCameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // get movement vector relative to world based on direction model is facing
                Vector3 movementVector = new Vector3(transform.forward.x, 0, transform.forward.z);

                // multiply by speed
                movementVector = movementVector * movementSpeed;

                // maintain vertical velocity
                movementVector = new Vector3(movementVector.x, playerRigidbody.velocity.y, movementVector.z);

                // apply movement vectors
                playerRigidbody.velocity = movementVector;

                // sound for stealth - moving sound
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, (5.0f * noiseModifier));
                foreach (var hitCollider in hitColliders)
                {
                    if(hitCollider.gameObject.tag == "Enemy")
                    {
                        hitCollider.gameObject.GetComponent<Pathing>().HeardNoise(transform.position);
                    }
                }
            }
            else
            {
                // cancel x and z velocity if no input
                playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y, 0);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = true;
            // sound for stealth - landing sound
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, (20.0f * noiseModifier));
            foreach (var hitCollider in hitColliders)
            {
                if(hitCollider.gameObject.tag == "Enemy")
                {
                    hitCollider.gameObject.GetComponent<Pathing>().HeardNoise(transform.position);
                }
            }
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}