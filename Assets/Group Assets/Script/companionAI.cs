using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class companionAI : MonoBehaviour
{
    public float stopDistance = 1;
    Transform player;
    NavMeshAgent nav;
    public CompanionState companionState;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        nav.stoppingDistance = stopDistance;
    }

    // Update is called once per frame
    void Update()
    {
        switch(companionState)
        {
            case CompanionState.Follow:
                if(nav.isStopped) nav.isStopped = false;
                nav.SetDestination(player.position);
                break;
            case CompanionState.Wait:
                nav.isStopped = true;
                break;
        }
    }

    public void changeState(string state)
    {
        switch(state)
        {
            case "Wait":
                companionState = CompanionState.Wait;
                break;
            case "Follow":
                companionState = CompanionState.Follow;
                break;
            case "Attack":
                companionState = CompanionState.Attack;
                break;
        }
    }

    public enum CompanionState
    {
        Wait,
        Follow,
        Attack
    }
}
