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
    private void Start()
    {
        _activeCam = SelectionCam;
    }
    public void ChangeCameraTo(CameraTypes type)
    {
        switch (type)
        {
            case CameraTypes.SelectionCam:
                _activeCam.Priority = 0;
                SelectionCam.Priority = 1;
                _activeCam = SelectionCam;
                break;
            case CameraTypes.PlayerFollowCam:
                _activeCam.Priority = 0;
                PlayerFollowCam.Priority = 1;
                _activeCam = PlayerFollowCam;
                break;
            case CameraTypes.HunterCam:
                _activeCam.Priority = 0;
                HunterCam.Priority = 1;
                _activeCam = HunterCam;
                break;
        }
    }
}
