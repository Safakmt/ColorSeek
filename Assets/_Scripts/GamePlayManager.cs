using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] GameObject HideButton;
    public static event Action OnGameStart;
    public static event Action OnHideButtonPressed;
    public GameState CurrentGameState { get; set; }

    private void OnEnable()
    {
        PlayerController.OnDestinationReached += PlayerDestinationReached;
    }
    private void Start()
    {
        cameraManager.ChangeCameraTo(CameraTypes.SelectionCam);
        CurrentGameState = GameState.Start;
    }
    private void Update()
    {

        if (CurrentGameState == GameState.Start && Input.GetMouseButtonDown(0)) 
        {
            cameraManager.ChangeCameraTo(CameraTypes.PlayerFollowCam);
            OnGameStart?.Invoke();
            CurrentGameState= GameState.Hide;
        }
        if (Input.GetMouseButtonDown(1))
        {
            cameraManager.ChangeCameraTo(CameraTypes.HunterCam);
            Hunter.SetActive(true);
        }

    }
    private void PlayerDestinationReached(bool state)
    {
        HideButton.SetActive(state);
    }
    public void HideButtonPressed()
    {
        OnHideButtonPressed?.Invoke();
        HideButton.SetActive(false);
    }
}
