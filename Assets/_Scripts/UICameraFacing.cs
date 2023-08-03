using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraFacing : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
