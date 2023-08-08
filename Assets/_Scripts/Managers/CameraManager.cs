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

    public static CameraManager Instance { get; private set; }

    private void OnEnable()
    {
        EventManager.OnHunterScream += ShakeCamera;
    }
    private void OnDisable()
    {
        EventManager.OnHunterScream -= ShakeCamera;    
    }
    private void Awake()
    {
        Instance = this;
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
        HunterCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 2f;
        HunterCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 4f;
        DOVirtual.DelayedCall(1.5f, () =>
        {
            HunterCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            HunterCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
        });
    }

    public void SetHunterCameraPos(Vector3 position)
    {
        HunterCam.transform.position = position;
    }
    public void SetHunterCamLookAt(Transform lookAtTransform)
    {
        HunterCam.LookAt = lookAtTransform;
    }
    public void SetSelectionFollowAndLookAt(Transform lookAtTransform)
    {
        SelectionCam.LookAt = lookAtTransform;
        SelectionCam.Follow = lookAtTransform;
    }

    public void SetPlayerFollowCamPosition(Transform transform)
    {
        PlayerFollowCam.transform.position = transform.position;
    }
}
