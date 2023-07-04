using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Start,
    Hide,
    Seek,
    End,
}
public class GamePlayManager : MonoBehaviour
{
    [SerializeField] CameraManager cameraManager;
    [SerializeField] GameObject Hunter;
    public static event Action OnGameStart;
    public GameState CurrentGameState { get; set; }


    private void Start()
    {
        CurrentGameState = GameState.Start;
        cameraManager.ChangeCameraTo(CameraTypes.SelectionCam);
    }
    private void Update()
    {

        if (CurrentGameState == GameState.Start && Input.GetMouseButtonDown(0)) 
        {
            cameraManager.ChangeCameraTo(CameraTypes.PlayerFollowCam);
            OnGameStart?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            cameraManager.ChangeCameraTo(CameraTypes.HunterCam);
            Hunter.SetActive(true);
        }
    }
}
