using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Moving,
    Hide,
    Escaping
}
public enum PoseType
{
    Tpose
}
public class AiMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private HideController _hideController;
    [SerializeField] private AnimatorController _animatorController;

    [Header("Values")]
    [SerializeField]private float _randomness;
    [SerializeField] private Transform destination;
    private AIState _currentState;
    public bool IsReached { get; set; }
    private void Start()
    {
        ResetAI();
    }
    private void OnEnable()
    {
        EventManager.OnGameStart += StartMovement;
        EventManager.OnSeekState += OnHide;
        EventManager.OnRefrencesSet += ResetAI;
    }
    private void OnDisable()
    {
        EventManager.OnGameStart -= StartMovement;
        EventManager.OnSeekState -= OnHide;
        EventManager.OnRefrencesSet -= ResetAI;
    }

    private void Update()
    {

        if (_currentState == AIState.Moving && agent.enabled)
        {
            MovingStateActivities();
        }

        if (_currentState == AIState.Idle && agent.enabled)
        {
            IdleStateActivities();
        }
        if (_currentState == AIState.Hide)
        {
            HideStateActivities();
        }
        if (_currentState == AIState.Escaping)
        {
            agent.enabled = false;
            _animatorController.PlayTPoseAnim();
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
            IsReached = false;
        }
    }
    private void ResetAI()
    {
        transform.DOKill();
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        agent.enabled = false;
        _currentState = AIState.Idle;
        IsReached = false;
        _animatorController.PlayIdleAnim();
        _hideController.Unhide();
    }
    private void SearchForNewLocation()
    {
        Vector3 distance = destination.position - transform.position;
        Vector3 randomVector = Random.onUnitSphere * Random.Range(0, _randomness+1);
        Vector3 targetPos = transform.position + Random.Range(0,_randomness+1) * distance.normalized;
        targetPos += randomVector;
        agent.destination = targetPos;
        _randomness /= 1.3f;
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

        if (_currentState == AIState.Hide)
        {
            return;
        }
        else
        {
            transform.DOKill();
            _currentState = AIState.Escaping;
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
    public void ToggleAgent(bool state)
    {
        agent.enabled = state;
        if (state)
        {
            agent.destination = destination.position;
        }
    }

}
