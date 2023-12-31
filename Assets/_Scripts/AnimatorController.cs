using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private const string RUN = "Run";
    private const string IDLE = "Idle";
    private const string TPOSE = "Tpose";
    private const string JUMP = "Jump";
    private string ActiveAnim;
    private string HidingAnim;
    private void Start()
    {
        ActiveAnim = IDLE;
    }
    public void PlayRunAnim()
    {
        _animator.SetBool(ActiveAnim, false);
        _animator.SetBool(RUN, true);
        ActiveAnim = RUN;
    }
    public void PlayIdleAnim()
    {
        _animator.SetBool(ActiveAnim, false);
        _animator.SetBool(IDLE, true);
        ActiveAnim = IDLE;
    }
    public void PlayTPoseAnim()
    {
        _animator.SetBool(ActiveAnim, false);
        _animator.SetBool(TPOSE, true);
        ActiveAnim = TPOSE;
    }
    public void SetHidingAnim()
    {

    }

    public void PlayJumpAnim()
    {
        _animator.SetTrigger(JUMP);
    }
}
