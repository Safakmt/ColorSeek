using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour, ISticker
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private Transform playerVisual;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform point;
    private Vector3 _gravity = new Vector3(0, -9.81f,0);
    bool _isMoving = false;
    public bool IsReached {
        get { return IsReached; }
        set {
            IsReached = value;
            //OnDestinationReached?.Invoke(IsReached);
        }
    }
    public bool isTakingInput { get; set; }

    public static event Action<bool> OnDestinationReached;
    private void Start()
    {
        isTakingInput = true;

    }
    private void Update()
    {
        Vector3 input = new Vector3(joystick.Horizontal,0,joystick.Vertical);

        if (Mathf.Abs(input.x) > 0.1f || Mathf.Abs(input.z) > 0.1f && isTakingInput) 
        {
            Move(input);
            Rotation(input);
            _isMoving = true;
            IsReached = false;
        }
        else
        {
            _isMoving = false;
        }

        if (!_isMoving && point != null)
        {
            transform.position = point.position;
            IsReached = true;
        }
        if (!characterController.isGrounded)
        {
            characterController.Move(_gravity * Time.deltaTime);
        }
    }

    public void SetStickPoint(Transform stickPoint)
    {
        point = stickPoint;
    }
    public void ClearStickPoint()
    {
        point = null;
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

    public bool IsOnRightPoint()
    {
        throw new NotImplementedException();
    }
}
