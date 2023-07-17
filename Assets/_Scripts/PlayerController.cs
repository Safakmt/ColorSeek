using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public enum PlayerState
{
    Idle,
    Moving,
    Hide
}
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private Transform playerVisual;
    [SerializeField] private HideController _hideController;
    [SerializeField] private AnimatorController _animatorController;

    [Header("Values")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    public event Action OnPlayerHide;
    public event Action OnPlayerUnhide;
    private Vector3 _gravity = new Vector3(0, -9.81f,0);
    private PlayerState _currentState;
    private bool _isMoving = false;
    private bool IsReached = false;
    private Vector3 _inputData = new Vector3();
    private bool _isHiding = false;
    private bool _isTakingInput;
    public bool IsTakingInput {
        get
        {
            return _isTakingInput;
        }
        set
        {
            _isTakingInput = value;
            joystick.enabled = value;
        } 
    }

    private void Start()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
        IsTakingInput = true;
        _currentState = PlayerState.Moving;
    }
    private void Update()
    {
        _inputData = new Vector3(joystick.Horizontal,0,joystick.Vertical);
        
        if (_currentState == PlayerState.Moving)
        {
            MovingStateActivities();
        }

        if (_currentState == PlayerState.Idle)
        {
            IdleStateActivities();
        }

        if (_currentState == PlayerState.Hide)
        {
            HideStateActivites();
        }

    }

    private void HideStateActivites ()
    {
        if (Mathf.Abs(_inputData.x) > 0.1f || Mathf.Abs(_inputData.z) > 0.1f && IsTakingInput)
        {
            _hideController.Unhide();
            transform.rotation = Quaternion.Euler(Vector3.zero);
            characterController.enabled = true;
            _currentState = PlayerState.Moving;
            _isHiding= false;
            OnPlayerUnhide?.Invoke();
        }
        else if(_isHiding)
        {
            
        }
        else
        {
            playerVisual.rotation = Quaternion.Euler(Vector3.zero);
            _hideController.Hide();
            characterController.enabled = false;
            _animatorController.PlayTPoseAnim();
            _isHiding = true;
            OnPlayerHide?.Invoke();
        }
    }
    private void IdleStateActivities()
    {
        _isMoving = false;
        _animatorController.PlayIdleAnim();
        if (Mathf.Abs(_inputData.x) > 0.1f || Mathf.Abs(_inputData.z) > 0.1f && IsTakingInput)
        {
            _currentState = PlayerState.Moving;
        }
        else if (_hideController.IsReadyToHide())
        {

            _currentState = PlayerState.Hide;
        }
        ApplyGravity();

    }
    private void MovingStateActivities() {

        if (Mathf.Abs(_inputData.x) > 0.1f || Mathf.Abs(_inputData.z) > 0.1f && IsTakingInput)
        {
            Move(_inputData);
            Rotation(_inputData);
            _animatorController.PlayRunAnim();
        }
        else if (_isMoving)
        {
            _currentState = PlayerState.Idle;
        }
        ApplyGravity();
    }
    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            characterController.Move(_gravity * Time.deltaTime);
        }
    }


    #region Move And Rotate Methods
    private void Rotation(Vector3 input)
    {
        Vector3 forward = followCamera.transform.forward;
        Vector3 right = followCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        Vector3 forwardRelativeVerticalInput = input.z * forward.normalized;
        Vector3 rightRelativeHorizontalInput = input.x * right.normalized;

        Vector3 cameraRelativeRotationInput = forwardRelativeVerticalInput + rightRelativeHorizontalInput;

        Quaternion lookRot = Quaternion.LookRotation(cameraRelativeRotationInput);
        playerVisual.rotation = Quaternion.RotateTowards(playerVisual.rotation,lookRot, rotationSpeed * Time.deltaTime);
    }

    private void Move(Vector3 input)
    {
        Vector3 forward = transform.InverseTransformVector(followCamera.transform.forward).normalized;
        Vector3 right = transform.InverseTransformVector(followCamera.transform.right).normalized;
        
        forward.y = 0;
        right.y = 0;
        
        Vector3 forwardRelativeVerticalInput = input.z * forward.normalized; 
        Vector3 rightRelativeHorizontalInput = input.x * right.normalized;

        Vector3 cameraRelativeMoveInput = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
        characterController.Move( cameraRelativeMoveInput * moveSpeed * Time.deltaTime);

        _isMoving = true;

    }
    #endregion
}
