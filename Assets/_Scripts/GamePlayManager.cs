using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float playTime;
    [SerializeField] private TextMeshProUGUI playTimeText;
    
    public GameState CurrentGameState { get; set; }

    private void OnEnable()
    {
        _playerController.OnPlayerHide += ShowHideButton;
        _playerController.OnPlayerUnhide += HideHideButton;
    }

    private void OnDisable()
    {
        _playerController.OnPlayerHide -= ShowHideButton;
        _playerController.OnPlayerUnhide -= HideHideButton;
        
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
            EventManager.GameStart();
            cameraManager.ChangeCameraTo(CameraTypes.PlayerFollowCam);
            CurrentGameState= GameState.Hide;
        }
        if (CurrentGameState == GameState.Hide)
        {
            playTime -= Time.deltaTime;
            playTimeText.text = playTime.ToString("0.0");
            if (playTime <= 0)
            {
                playTimeText.text = "0";
                CurrentGameState = GameState.Seek;
            }
        }

        if (CurrentGameState == GameState.Seek)
        {
            if (!Hunter.activeSelf)
            {
                EventManager.SeekState();
                Hunter.SetActive(true);
                cameraManager.ChangeCameraTo(CameraTypes.HunterCam);
                HideButton.SetActive(false);

            }
        }

    }
    private void ShowHideButton()
    {
        HideButton.SetActive(true);
    }
    private void HideHideButton()
    {
        HideButton.SetActive(false);
    }
    public void HideButtonPressed()
    {
        playTime = 0;
    }
}
