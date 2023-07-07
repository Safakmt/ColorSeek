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
    [SerializeField] private AnimatorController _animatorController;
    private AIState _currentState;
    public bool IsReached { get; set; }
    private void Start()
    {
        IsReached = false;
        agent.enabled = false;
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
            MovingStateActivities();
        }

        if (_currentState == AIState.Idle)
        {
            IdleStateActivities();
        }
        if (_currentState == AIState.Hide)
        {
            HideStateActivities();
        }
    }
    private void MovingStateActivities()
    {
        _animatorController.PlayRunAnim();
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            _currentState = AIState.Idle;
            IsReached = true;
        }
    }
    private void IdleStateActivities()
    {
        if (_hideController.IsReadyToHide())
        {
            _currentState = AIState.Hide;
        }
        else
        {
            SearchForNewLocation();
            _currentState = AIState.Moving;
        }
    }

    private void SearchForNewLocation()
    {
        
    }

    private void HideStateActivities()
    {
        agent.enabled = false;
        _animatorController.PlayTPoseAnim();
        _hideController.Hide();
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
        agent.enabled = true;
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
