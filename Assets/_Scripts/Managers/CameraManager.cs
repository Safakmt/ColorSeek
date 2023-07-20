using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes
{
    SelectionCam,
    PlayerFollowCam,
    HunterCam,
    StartCam
}
public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera SelectionCam;
    [SerializeField] private CinemachineVirtualCamera PlayerFollowCam;
    [SerializeField] private CinemachineVirtualCamera HunterCam;
    [SerializeField] private CinemachineVirtualCamera StartCam;
    private CinemachineVirtualCamera _activeCam;

    private void Awake()
    {
        _activeCam = StartCam;
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
            case CameraTypes.StartCam:
                _activeCam = StartCam;
                break;
        }

        _activeCam.Priority = 1;
    }

    public void ShakeCamera()
    {
        HunterCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1f;
        HunterCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1f;
        DOVirtual.DelayedCall(1f, () =>
        {
            HunterCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            HunterCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
        });
    }
}
