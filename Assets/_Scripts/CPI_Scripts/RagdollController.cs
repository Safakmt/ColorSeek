using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> _bodies = new List<Rigidbody>();
    [SerializeField] private List<Collider> _colliders = new List<Collider>();
    [SerializeField] private Collider _mainCollider;
    [SerializeField] private Rigidbody _mainRigid;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        RagdollToggle(false);
    }
    public void RagdollToggle(bool state)
    {
        foreach (var body in _bodies)
        {
            body.isKinematic = !state;
        }
        foreach (var col in _colliders)
        {
            col.enabled = state;
        }

        _mainCollider.enabled = !state;
        //_mainRigid.isKinematic = true;
        _animator.enabled = !state;
    }
}
