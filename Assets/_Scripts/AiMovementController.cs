using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Moving,
    Searching,
    Hide
}
public class AiMovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform destination;
    [SerializeField] private HideController _hideController;
    private AIState _currentState;
    public bool IsReached { get; set; }
    private void Start()
    {
        IsReached = false;
        _currentState = AIState.Moving;
    }
    private void OnEnable()
    {
        EventManager.OnGameStart += StartMovement;
        GamePlayManager.OnHideButtonPressed += OnHide;
    }
    private void OnDisable()
    {
        EventManager.OnGameStart -= StartMovement;
        GamePlayManager.OnHideButtonPressed -= OnHide;
    }
    private void Update()
    {

        if (_currentState == AIState.Moving && agent.enabled)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && _hideController.IsReadyToHide())
            {
                _currentState = AIState.Hide;
                IsReached = true;
            }
        }

        if (_currentState == AIState.Hide)
        {
            agent.enabled = false;
            _hideController.Hide();
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

}
