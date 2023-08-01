using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameObject Hunter;
    [SerializeField] private GameObject HideButton;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float playTime;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private Transform characterCreatePivot;
    [SerializeField] private HidingSpotAssigner _hidingSpotAssigner;
    private bool isHideButtonPressed = false;
    public GameState CurrentGameState { get; set; }
    public static GamePlayManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        EventManager.OnPlayerHide += ShowHideButton;
        EventManager.OnPlayerUnhide += HideHideButton;
        EventManager.OnEnvironmentInitalized += OnEnvironmentLoaded;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerHide -= ShowHideButton;
        EventManager.OnPlayerUnhide -= HideHideButton;
        EventManager.OnEnvironmentInitalized -= OnEnvironmentLoaded;
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
            if (playTime <= 0 || isHideButtonPressed)
            {
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

    

    private void StartNextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
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
        isHideButtonPressed= true;
        _playerController.StopTakingInput();
    }

    public void PlaceCircular()
    {
        HideController[] hiders = _hidingSpotAssigner.GetHideControllers();
        for (int i = 0; i < hiders.Length; i++)
        {
            float radius = hiders.Length;
            float angle = i * Mathf.PI * 2f / radius;
            Vector3 pos = characterCreatePivot.position + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
            hiders[i].transform.position = pos;
        }
    }

    private void ResetGamePlay()
    {
        cameraManager.ChangeCameraTo(CameraTypes.SelectionCam);
        CurrentGameState = GameState.Start;
        Hunter.SetActive(false);
        isHideButtonPressed = false;
        HideHideButton();
        playTimeText.text = "";
        PlaceCircular();
    }
    private void OnDrawGizmos()
    {
        if (characterCreatePivot == null)
            return;
        for (int i = 0; i < 5; i++)
        {
            float radius = 5;
            float angle = i * Mathf.PI *2f / radius;
            Gizmos.DrawWireSphere(characterCreatePivot.position + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))),0.3f);
        }
    }
    private void GetCurrentPlayTime()
    {
        playTime = LevelManager.Instance.GetCurrentLevelData().PlayTime;
        playTimeText.text = playTime.ToString();
    }
    public void OnEnvironmentLoaded(EnvironmentData environmentData)
    {
        LevelManager.Instance.activeEnvData = environmentData;
        _hidingSpotAssigner.SetHideSpotList(environmentData.hidingSpots);
        cameraManager.SetHunterCameraPos(environmentData.hunterCamPos.position);
        Hunter.transform.position = environmentData.hunterSpawnPos.position;
        characterCreatePivot = environmentData.charSpawnPos;
        cameraManager.SetSelectionFollowAndLookAt(characterCreatePivot);
        GetCurrentPlayTime();
        ResetGamePlay();
        EventManager.RefrencesSet();
    }
}
