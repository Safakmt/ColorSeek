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
    public static event Action OnGameStart;
    public GameState CurrentGameState { get; set; }
    public CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera StartCam;
    public CinemachineVirtualCamera PlayCam;
    public CinemachineVirtualCamera HunterCam;

    private void Start()
    {
        CurrentGameState = GameState.Start;
        StartCam.Priority = 1;
        PlayCam.Priority = 0;
        HunterCam.Priority = 0;
    }
    private void Update()
    {

        if (CurrentGameState == GameState.Start && Input.GetMouseButtonDown(0)) 
        {
            StartCam.Priority = 0;
            PlayCam.Priority = 1 ;
            OnGameStart?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            PlayCam.Priority = 0;
            HunterCam.Priority= 1;
        }
    }
}
