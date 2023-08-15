using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CatchPlayer : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private Animator _animator;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("Catch");
        }
    }
}
