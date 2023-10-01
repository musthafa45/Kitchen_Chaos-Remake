using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager Instance { get; private set; }


    private NavMeshAgent agent;
    private Vector3 destination;
    private Action OnTargetReached;
    private bool isReachedPosition;

    private void Awake()
    {
        Instance = this;
    }
    public void SetAgent(NavMeshAgent agent,Vector3 destination,Action OnTargetReached = null)
    {
        this.agent = agent;
        this.destination = destination;
        this.OnTargetReached = OnTargetReached;
    }

    private void Update()
    {
        if (agent == null) return;

        if (agent != null && destination != null)
        {
            agent.SetDestination(destination);
        }
        if(HasReachedPosition() && !isReachedPosition)
        {
            OnTargetReached();
            isReachedPosition = true;
        }
       
    }

    private bool HasReachedPosition()
    {
        float distance = Vector3.Distance(agent.transform.position, destination);

        return distance < .1f;
    }
}
