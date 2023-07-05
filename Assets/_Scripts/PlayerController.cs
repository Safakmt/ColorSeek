using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerState
{
    Idle,
    Moving,
    Hide
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private Transform playerVisual;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private HideController _hideController;
    private Vector3 _gravity = new Vector3(0, -9.81f,0);
    private PlayerState _currentState;
    bool _isMoving = false;
    bool IsReached = false;
    public bool isTakingInput { get; set; }

    private void Start()
    {
        isTakingInput = true;
        _currentState = PlayerState.Moving;
    }
    private void Update()
    {
        Vector3 input = new Vector3(joystick.Horizontal,0,joystick.Vertical);
        
        if (_currentState == PlayerState.Moving)
        {
            if (Mathf.Abs(input.x) > 0.1f || Mathf.Abs(input.z) > 0.1f && isTakingInput) 
            {
                Move(input);
                Rotation(input);
                _isMoving = true;
            }
            else if (_isMoving)
            {
                StopPlayer();
            }
        }

        if (_currentState == PlayerState.Hide)
        {

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

    private void StopPlayer()
    {
        _isMoving = false;
        if (_hideController.IsReadyToHide())
        {
            _hideController.Hide();
            _currentState = PlayerState.Hide;
        }
    }

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
    }

}
