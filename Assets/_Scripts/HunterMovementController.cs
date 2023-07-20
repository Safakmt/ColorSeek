using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;
using static Cinemachine.CinemachineOrbitalTransposer;

public enum HunterState
{
    Walk,
    Catch
}
public class HunterMovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerController player;
    [SerializeField] private int seekingCount;
    [SerializeField] private Transform _catchingSpot;
    [SerializeField] private Transform _catchingObjectPosition;
    [SerializeField] private Animator _animator;
    private List<HideController> _hidingList = new List<HideController>();
    private List<HideController> _seekedList = new List<HideController>();
    private HideController currentChased;
    private Transform currentChasedTransform;
    private HunterState _currentState;
    private bool isScreaming = false;
    private void OnEnable()
    {
        EventManager.OnHunterCatch += OnCatchAnimationEvent;
        _animator.SetTrigger("Scream");
        isScreaming = true;
        DOVirtual.DelayedCall(2.8f, () =>
        {
            _currentState = HunterState.Walk;
            isScreaming= false;
        });
    }
    private void OnDisable()
    {
        EventManager.OnHunterCatch -= OnCatchAnimationEvent;
    }
    private void Start()
    {
        HideController[] controllers = FindObjectsOfType<HideController>();
        foreach (HideController controller in controllers)
        {
            _hidingList.Add(controller);
        }

    }

    private void Update()
    {
        if (_currentState == HunterState.Walk && !isScreaming)
        {
            WalkStateActivites();
        }

        if (_currentState == HunterState.Catch && !isScreaming)
        {
            CatchStateActivities();
        }
    }

    void WalkStateActivites()
    {
        if (currentChased == null && seekingCount > 0)
        {
            currentChased = _hidingList[Random.Range(0, _hidingList.Count)];
            _animator.SetTrigger("Walk");
        }
        else if (currentChased != null)
        {
            agent.destination = currentChased.transform.position;
            if (Vector3.Distance(_catchingSpot.position, currentChased.transform.position) <= 5f)
            {
                _currentState = HunterState.Catch;
                _hidingList.Remove(currentChased);
                _seekedList.Add(currentChased);
            }
        }
        else if (currentChased == null && seekingCount == 0)
        {
            _animator.SetBool("Idle",true);
        }
    }

    void CatchStateActivities()
    {
        if (currentChased != null)
        {
            currentChasedTransform = currentChased.transform;
            agent.SetDestination(transform.position);
            transform.LookAt(currentChased.transform.position);
            _animator.SetTrigger("Catch");  //OnCatchAnimationEvent trigger
            GameObject deactive = currentChased.gameObject;
            DOVirtual.DelayedCall(2f, () =>
            {
                _currentState = HunterState.Walk;
                deactive.SetActive(false);
            });
    
        currentChased = null;
        seekingCount -= 1;
        }
        if (seekingCount == 0)
        {
            _animator.SetTrigger("Scream");
            isScreaming = true;
            DOVirtual.DelayedCall(2.8f, () =>
            {
                _currentState = HunterState.Walk;
                isScreaming = false;
            });
        }
    }
    public void OnCatchAnimationEvent()
    {
        currentChasedTransform.SetParent(_catchingObjectPosition);
        currentChasedTransform.localPosition = Vector3.zero;

    }
    private void OnDrawGizmosSelected()
    {
        if (currentChased!=null)
        {
            Gizmos.DrawRay(_catchingSpot.position,currentChased.transform.position);

        }
    }
}
