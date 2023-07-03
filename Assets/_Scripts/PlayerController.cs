using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour, ISticker
{
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] CharacterController characterController;
    [SerializeField] CinemachineVirtualCamera followCamera;
    [SerializeField] Transform playerVisual;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform point;
    Vector3 gravity = new Vector3(0, -9.81f,0);
    bool isMoving = false;
    bool isTakingInput = false;
    GamePlayManager gamePlayManager;
    private void Start()
    {
        gamePlayManager= FindObjectOfType<GamePlayManager>();
        isTakingInput = true;

    }
    private void Update()
    {
        Vector3 input = new Vector3(joystick.Horizontal,0,joystick.Vertical);

        if (Mathf.Abs(input.x) > 0.1f || Mathf.Abs(input.z) > 0.1f && isTakingInput) 
        {
            gamePlayManager.CurrentGameState = GameState.Hide;
            Move(input);
            Rotation(input);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (!isMoving && point != null)
        {
            transform.position = point.position;
        }

        if (!characterController.isGrounded)
        {
            characterController.Move(gravity * Time.deltaTime);
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

}
