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
public enum PoseType
{
    Tpose
}
public class AiMovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform destination;
    [SerializeField] private HideController _hideController;
    [SerializeField] private AnimatorController _animatorController;
    private AIState _currentState;
    public float Randomness;
    public bool IsReached { get; set; }
    private void Start()
    {
        IsReached = false;
        agent.enabled = false;
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
            if (agent.enabled)
                SearchForNewLocation();
            _currentState = AIState.Moving;
        }
    }

    private void SearchForNewLocation()
    {
        Vector3 distance = destination.position - transform.position;
        Vector3 randomVector = Random.onUnitSphere * Random.Range(0, Randomness+1);
        Vector3 targetPos = transform.position + Random.Range(0,Randomness+1) * distance.normalized;
        targetPos += randomVector;
        agent.destination = targetPos;
        Debug.Log(transform.name +targetPos);
        Randomness /= 1.3f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, agent.destination);
    }
    private void HideStateActivities()
    {
        if (agent.enabled)
            _hideController.Hide();
        _animatorController.PlayTPoseAnim();
        agent.enabled = false;
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
        SetDestination(_hideController.GetRightSpot());
        _currentState = AIState.Idle;
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
