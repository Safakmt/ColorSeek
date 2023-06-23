using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiMovementController : MonoBehaviour, ISticker
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform point;
    [SerializeField] private Transform destination;

    private void Start()
    {
        agent.destination = destination.position;
    }
    private void Update()
    {

        if (agent.enabled)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && point != null)
            {
                agent.enabled = false;
                transform.position = point.position;
            }
        }
    }
    public void ClearStickPoint()
    {
        point = null;
    }

    public void SetStickPoint(Transform stickPoint)
    {
        point = stickPoint;
    }

    public void ToggleNavmesh(bool state)
    {
        agent.enabled = state;
        if (state)
        {
            agent.destination = destination.position;
        }
    }
}
