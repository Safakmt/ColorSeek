using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiMovementController : MonoBehaviour, ISticker
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform point;
    [SerializeField] private Transform destination;
    public bool IsReached { get; set; }

    private void Start()
    {
        IsReached = false;
    }
    private void OnEnable()
    {
        GamePlayManager.OnGameStart += StartMovement;
        GamePlayManager.OnHideButtonPressed += OnHide;
    }
    private void OnDisable()
    {
        GamePlayManager.OnGameStart -= StartMovement;
        GamePlayManager.OnHideButtonPressed -= OnHide;
    }
    private void Update()
    {

        if (agent.enabled)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && point != null)
            {
                agent.enabled = false;
                transform.position = point.position;
                IsReached = true;
            }
        }
    }

    private void OnHide()
    {
        if (!IsReached)
        {
            transform.DOKill();
            agent.enabled = false;
            transform.position = destination.position;
            agent.enabled = true;
        }
    }
    private void StartMovement()
    {
        agent.destination = destination.position;
    }
    public void ClearStickPoint()
    {
        point = null;
    }

    public void SetStickPoint(Transform stickPoint)
    {
        point = stickPoint;
    }

    public void SetDestination(Transform destination)
    {
        this.destination = destination;
    }
    public void ToggleNavmesh(bool state)
    {
        agent.enabled = state;
        if (state)
        {
            agent.destination = destination.position;
        }
    }

    public bool IsOnRightPoint()
    {
        throw new System.NotImplementedException();
    }
}
