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

    public GameObject PlayerUI;

    public GameObject initialPatrolPoint;
    private GameObject currentPatrolPoint;
    public GameObject weapon;
    private EnemyWeapon MyWeapon;
    public int firingDistance;

    void Start()
    {
        seeTarget = false;
        heardSomething = false;
        // stay still on start
        destination = transform.position;
        foundTarget = false;
        currentPatrolPoint = initialPatrolPoint;
        MyWeapon = weapon.GetComponent<EnemyWeapon>();
    }

    private void Fire()
    {
        MyWeapon.fire = true;
    }

    private void StopFiring()
    {
        MyWeapon.fire = false;
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
                DetectionBar detection = PlayerUI.transform.Find("Detection Bar").GetComponent<DetectionBar>();
                VisibilityBar visibility = PlayerUI.transform.Find("Visibility Bar").GetComponent<VisibilityBar>();
                float distance = Vector3.Distance(visibleTargets[0].position, transform.position);
                if (detection != null)
                {
                    detection.AddDetection((visibility.slider.normalizedValue + 0.01f) * Time.deltaTime);
                    if (distance < 3) // if close to target immediately see it
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
                // look directly at target
                transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
                // set destination as target position
                SetDestination(target.position);
                // shoot if close enougth
                float distance = Vector3.Distance(target.position, transform.position);
                if(distance < firingDistance)
                {
                    Fire();
                }
                else
                {
                    StopFiring();
                }
            }
            else
            {
                StopFiring();
            }
        }
        else // if lose sight of target
        {
            foundTarget = false;
            // if haven't heard anything
            if (!heardSomething)
            {
                // passive patrol route when starting to detect
                SetDestination(currentPatrolPoint.transform.position); // go to current patorl location
                if (agent.remainingDistance < agent.stoppingDistance) // if reach current patorl location find next one
                {
                    currentPatrolPoint = currentPatrolPoint.GetComponent<PatrolSystem>().nextPoint;
                }
            }
            else
            {
                if (agent.remainingDistance < agent.stoppingDistance) // if reach current patorl location find next one
                {
                    heardSomething = false;
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
