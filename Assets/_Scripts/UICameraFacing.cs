using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraFacing : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cameraTransform.position);
    }
}
