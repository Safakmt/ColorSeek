using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes
{
    SelectionCam,
    PlayerFollowCam,
    HunterCam
}
public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera SelectionCam;
    [SerializeField] private CinemachineVirtualCamera PlayerFollowCam;
    [SerializeField] private CinemachineVirtualCamera HunterCam;
    private CinemachineVirtualCamera _activeCam;
    private void Awake()
    {
        _activeCam = SelectionCam;
        PlayerController player = FindObjectOfType<PlayerController>();
        PlayerFollowCam.Follow = player.transform;
        PlayerFollowCam.LookAt= player.transform;
    }
    public void ChangeCameraTo(CameraTypes type)
    {
        _activeCam.Priority = 0;
        switch (type)
        {
            case CameraTypes.SelectionCam:
                _activeCam = SelectionCam;
                break;
            case CameraTypes.PlayerFollowCam:
                _activeCam = PlayerFollowCam;
                break;
            case CameraTypes.HunterCam:
                _activeCam = HunterCam;
                break;
        }

        _activeCam.Priority = 1;
    }
}
