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
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject Hunter;
    [SerializeField] GameObject HideButton;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float playTime;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private Transform characterCreatePivot;
    public GameState CurrentGameState { get; set; }

    private void OnEnable()
    {
        EventManager.OnPlayerHide += ShowHideButton;
        EventManager.OnPlayerUnhide += HideHideButton;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerHide -= ShowHideButton;
        EventManager.OnPlayerUnhide -= HideHideButton;
        
    }
    private void Start()
    {
        cameraManager.ChangeCameraTo(CameraTypes.SelectionCam);
        CurrentGameState = GameState.Start;
        PlaceCircular(FindObjectsOfType<HideController>());
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

    public void PlaceCircular(HideController[] hiders)
    {
        for (int i = 0; i < hiders.Length; i++)
        {
            float radius = hiders.Length;
            float angle = i * Mathf.PI * 2f / radius;
            Vector3 pos = characterCreatePivot.position + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
            hiders[i].transform.position = pos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < 5; i++)
        {
            float radius = 5;
            float angle = i * Mathf.PI *2f / radius;
            Gizmos.DrawWireSphere(characterCreatePivot.position + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))),0.3f);
        }
    }
}
