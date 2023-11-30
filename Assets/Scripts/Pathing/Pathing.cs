using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathing : MonoBehaviour
{
    public NavMeshAgent agent;
    private Vector3 destination;
    private bool seeTarget;
    private bool heardSomething;
    private List<Transform> visibleTargets = new List<Transform>();

    private bool foundTarget;

    void Start()
    {
        seeTarget = false;
        heardSomething = false;
        // stay still on start
        destination = transform.position;
        foundTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if can see a target
        if (seeTarget)
        {
            // logic to pick from visable targets
            Transform target = visibleTargets[0];
            for (int i = 0; i < visibleTargets.Count; i++) // for all visible
            {
                // fill detection bar while in vision
                DetectionBar detection = visibleTargets[i].Find("PlayerUI").Find("Detection Bar").GetComponent<DetectionBar>();
                VisibilityBar visibility = visibleTargets[i].Find("PlayerUI").Find("Visibility Bar").GetComponent<VisibilityBar>();
                float distance = Vector3.Distance(visibleTargets[0].position, transform.position);
                if (detection != null)
                {
                    detection.AddDetection((visibility.slider.normalizedValue + 0.01f) * Time.deltaTime);
                    if(distance < 3) // if close to target immediately see it
                    {
                        detection.AddDetection(100.0f);
                    }
                }
                // if not found a target then 1st one to fill detection bar gets set as target
                if (!foundTarget)
                {
                    if (detection != null)
                    {
                        if (detection.slider.normalizedValue == 1.0f)
                        {
                            target = visibleTargets[i];
                            foundTarget = true;
                        }
                    }
                }
            }
            if (foundTarget) // if found something
            {
                Debug.Log("Found Target");
                // look directly at target
                transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
                // set destination as target position
                SetDestination(target.position);
            }
            else // if not found something
            {
                Debug.Log("Regaining Target");
                // passive patrol route when starting to detect
                transform.Rotate(new Vector3(0, 0.025f, 0)); // WIP placeholder that slowly rotates on the spot
            }
        }
        else // if lose sight of target
        {
            foundTarget = false;
            // if haven't heard anything
            if (!heardSomething)
            {
                // if have reached end of current path
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    Debug.Log("Lost Target");
                    // passive patrol route when cant see anything at all
                    transform.Rotate(new Vector3(0, 0.1f, 0)); // WIP placeholder that rotates on the spot
                }
            }
        }
        // update navmesh destination
        if (destination != null)
        {
            agent.SetDestination(destination);
        }
    }

    public void SetDestination(Vector3 target)
    {
        destination = target;
    }

    public void HeardNoise(Vector3 location)
    {
        // if can't target see then go look at what the noise was
        if (!seeTarget)
        {
            heardSomething = true;
            transform.LookAt(location);
            SetDestination(location);
        }
    }

    public void SeeSomething(bool canTheySeeSomthing, List<Transform> whatCanBeSeen)
    {
        seeTarget = canTheySeeSomthing;
        // if can see something get list of what can be seen
        if (canTheySeeSomthing)
        {
            heardSomething = false; // forget about any noises
            visibleTargets = whatCanBeSeen;
        }
    }
}
