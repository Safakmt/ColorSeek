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
    Catch,
    Idle
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
    private List<HideController> _huntingList = new List<HideController>();
    private List<HideController> _seekedList = new List<HideController>();
    private HideController currentChased;
    private Transform currentChasedTransform;
    private HunterState _currentState;
    private bool isScreaming = false;
    private Tween _catchAnimDelayedCall;
    private HideController _playerHideController;
    private int _huntingCount;
    private void OnEnable()
    {
        EventManager.OnHunterCatch += OnCatchAnimationEvent;
        transform.LookAt(Camera.main.transform.position);
        HunterScreamActivities();
        CreateHuntingList();
    }
    private void OnDisable()
    {
        EventManager.OnHunterCatch -= OnCatchAnimationEvent;
        ResetHunter();
    }
    private void Start()
    {
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
        if (_currentState == HunterState.Idle)
        {
            IdleStateActivities();
        }
    }

    private void ResetHunter()
    {
        _hidingList.Clear();
        _catchAnimDelayedCall.Kill();
        foreach (var seeked in _seekedList)
        {
            seeked.gameObject.SetActive(true);
            seeked.transform.SetParent(null);
        }
        _seekedList.Clear();
        _huntingList.Clear();
        currentChased = null;
        currentChasedTransform = null;
    }
    private void HunterScreamActivities()
    {
        _animator.SetTrigger("Scream");
        EventManager.HunterScream();
        isScreaming = true;
        DOVirtual.DelayedCall(2.8f, () =>
        {
            _currentState = HunterState.Walk;
            isScreaming = false;
        });
    }
    private void IdleStateActivities()
    {
        _animator.SetBool("Idle", true);
    }

    void WalkStateActivites()
    {
        if (currentChased == null && _huntingCount > 0)
        {
            currentChased = _huntingList[0];
            _huntingList.RemoveAt(0);
            _animator.SetTrigger("Walk");
        }
        else if (currentChased != null)
        {
            agent.destination = currentChased.transform.position;
            if (Vector3.Distance(_catchingSpot.position, currentChased.transform.position) <= 5f)
            {
                _currentState = HunterState.Catch;
                _seekedList.Add(currentChased);
            }
        }
        else if (currentChased == null && _huntingCount == seekingCount)
        {
            _currentState = HunterState.Idle;
        }
    }

    private void CreateHuntingList()
    {
        _huntingCount = seekingCount;

        HideController[] controllers = FindObjectsOfType<HideController>();
        _playerHideController = player.GetComponent<HideController>();

        int firstIndex = 0;
        foreach (HideController controller in controllers)
        {
            if(controller != _playerHideController)
            {
                if (!controller.IsHiding)
                {
                    _huntingList.Insert(0,controller);
                    firstIndex++;
                }
                else if (!controller.IsHidingRightSpot())
                {
                    _huntingList.Insert(firstIndex,controller);
                }
                else
                {
                    _huntingList.Add(controller);
                }
            }
        }
        if (!_playerHideController.IsHidingRightSpot() || !_playerHideController.IsHiding)
        {
            _huntingList.Insert(2,_playerHideController);
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
            currentChased = null;
            _huntingCount -= 1;

            _catchAnimDelayedCall = DOVirtual.DelayedCall(2f, () =>
            {
                deactive.SetActive(false);

            }).OnComplete(() =>
            {
                if (_huntingCount > 0)
                {
                    _currentState = HunterState.Walk;
                }
                else if(_huntingCount == 0)
                {
                    transform.LookAt(Camera.main.transform.position);
                    _animator.SetTrigger("Scream");
                    EventManager.HunterScream();
                    DOVirtual.DelayedCall(2.8f, () =>
                    {
                        EventManager.HuntingFinished();
                        Debug.Log("huntung finished");
                    });
                }
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
