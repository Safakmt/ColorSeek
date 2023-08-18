using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivy = 100f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed;
    [SerializeField] private float _snappiness;
    private float yRotation = 0f;
    private float xVelocity = 0f;
    private float yVelocity = 0f;
    private float mouseY = 0f;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        HunterMove();
        HunterLook();
    }

    private void HunterMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 newForward = transform.forward;
        newForward.y = 0;
        Vector3 move = transform.right * x + newForward * z;
        _controller.Move(move * _speed * Time.deltaTime);
    }

    private void HunterLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivy * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivy * Time.deltaTime;


        xVelocity = Mathf.Lerp(xVelocity, mouseX, _snappiness * Time.deltaTime);
        yVelocity = Mathf.Lerp(yVelocity, mouseY, _snappiness * Time.deltaTime);


        mouseY = Mathf.Clamp(mouseY, -90f, 90f);
        transform.localRotation = Quaternion.Euler(yVelocity, 0, 0);
        playerTransform.Rotate(Vector3.up, xVelocity);
    }
}
