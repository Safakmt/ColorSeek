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
    private HunterState _currentState;
    private void Start()
    {
        HideController[] controllers = FindObjectsOfType<HideController>();
        foreach (HideController controller in controllers)
        {
            _hidingList.Add(controller);
        }
        _currentState = HunterState.Walk;
    }

    private void Update()
    {
        if (_currentState == HunterState.Walk)
        {
            WalkStateActivites();
        }

        if (_currentState == HunterState.Catch)
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
        else if (currentChased != null)
        {
            _animator.SetTrigger("Idle");
        }
    }

    void CatchStateActivities()
    {
        if (currentChased != null)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(currentChased.transform.position);
            currentChased.transform.SetParent(_catchingObjectPosition);
            currentChased.transform.localPosition = Vector3.zero;
            _animator.SetTrigger("Catch");
            GameObject deactive = currentChased.gameObject;
            DOVirtual.DelayedCall(1f, () =>
            {
                _currentState = HunterState.Walk;
                deactive.gameObject.SetActive(false);
            });
    
        currentChased = null;
        seekingCount -= 1;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (currentChased!=null)
        {
            Gizmos.DrawRay(_catchingSpot.position,currentChased.transform.position);

        }
    }
}
