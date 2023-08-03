using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using TMPro.EditorUtilities;
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
    [SerializeField] private GameObject _hunter;
    [SerializeField] private HunterMovementController _hunterController;
    [SerializeField] private GameObject _hideButton;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private HideController _playerHideController;
    [SerializeField] private float _playTime;
    [SerializeField] private TextMeshProUGUI _playTimeText;
    [SerializeField] private Transform _characterCreatePivot;
    [SerializeField] private HidingSpotAssigner _hidingSpotAssigner;
    [SerializeField] private GameObject _selectionArrow;
    private bool isHideButtonPressed = false;
    private bool _isSelectionPartPlaying = false;
    private float _playerAngle;
    private List<HideController> hiders = new List<HideController>();
    private List<float> hiderAngles = new List<float>();
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
        hiders = _hidingSpotAssigner.GetHideControllers();
        CameraManager.Instance.ChangeCameraTo(CameraTypes.SelectionCam);
        CurrentGameState = GameState.Start;
    }
    private void Update()
    {

        if (CurrentGameState == GameState.Start && Input.GetMouseButtonDown(0) && !_isSelectionPartPlaying) 
        {
            PlaySelectionSection();

        }
        if (CurrentGameState == GameState.Hide)
        {
            _playTime -= Time.deltaTime;
            _playTimeText.text = _playTime.ToString("0.0");
            if (_playTime <= 0 || isHideButtonPressed)
            {
                CurrentGameState = GameState.Seek;
            }
        }

        if (CurrentGameState == GameState.Seek)
        {
            if (!_hunter.activeSelf)
            {
                EventManager.SeekState();
                _hunter.SetActive(true);
                CameraManager.Instance.ChangeCameraTo(CameraTypes.HunterCam);
                _hideButton.SetActive(false);
            } 
        }

    }

    private void ShowHideButton()
    {
        _hideButton.SetActive(true);
    }
    private void HideHideButton()
    {
        _hideButton.SetActive(false);
    }
    public void HideButtonPressed()
    {
        isHideButtonPressed= true;
        _playerController.StopTakingInput();
    }

    public void PlaceCircular()
    {
        Shuffle(hiders);
        for (int i = 0; i < hiders.Count; i++)
        {
            float degreeForEach = 360 / hiders.Count;
            hiderAngles.Add(degreeForEach * i);
            if (hiders[i] == _playerHideController)
                _playerAngle = degreeForEach* i;

            Vector3 pos = _characterCreatePivot.position + Quaternion.Euler(0, degreeForEach * i, 0) * Vector3.forward;
            hiders[i].transform.position = pos;
            hiders[i].GetName().enabled = false;
        }

    }

    private void PlaySelectionSection()
    {
        EventManager.SelectionStart();
        _isSelectionPartPlaying = true;
        _selectionArrow.transform.position = _characterCreatePivot.position;
        _selectionArrow.SetActive(true);
        float selectionDegree = 360 / hiderAngles.Count;
        selectionDegree = selectionDegree * 0.5f;
        _selectionArrow.transform.DORotate(new Vector3(0,360,0) * 3 +new Vector3(0,_playerAngle,0), 2.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                
                for (int i = 0; i < hiderAngles.Count; i++)
                {
                    float arrowYRotation = _selectionArrow.transform.localRotation.eulerAngles.y;

                    if (arrowYRotation > 360 - selectionDegree)
                        arrowYRotation = arrowYRotation - 360;

                    if (arrowYRotation > hiderAngles[i] - selectionDegree && arrowYRotation < hiderAngles[i] + selectionDegree)
                    {
                        hiders[i].ToggleSelection(true);
                    }
                    else
                        hiders[i].ToggleSelection(false);
                }
            })
            .OnComplete(() =>
        {
            float firstScale = _selectionArrow.transform.localScale.x;
            _selectionArrow.transform.DOScale(firstScale * 1.2f, 0.3f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                _selectionArrow.transform.DOScale(firstScale, 0.3f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _playerHideController.GetName().enabled = true;
                });
            });
        });

        DOVirtual.DelayedCall(4.3f, () =>
        {
            EventManager.GameStart();
            CameraManager.Instance.ChangeCameraTo(CameraTypes.PlayerFollowCam);
            CurrentGameState = GameState.Hide;
            _selectionArrow.SetActive(false);
            _playerHideController.ToggleSelection(false);
            for (int i = 0; i < hiders.Count; i++)
            {
                hiders[i].GetName().enabled = true;
            }
        });
    }

    private void ResetGamePlay()
    {
        CameraManager.Instance.ChangeCameraTo(CameraTypes.SelectionCam);
        _isSelectionPartPlaying = false;
        CurrentGameState = GameState.Start;
        _hunter.SetActive(false);
        isHideButtonPressed = false;
        HideHideButton();
        PlaceCircular();
    }
    private void OnDrawGizmos()
    {
        if (_characterCreatePivot == null)
            return;
        for (int i = 0; i < 5; i++)
        {
            float radius = 5;
            float angle = i * Mathf.PI *2f / radius;
            Gizmos.DrawWireSphere(_characterCreatePivot.position + (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))),0.3f);
        }
    }
    private void GetCurrentPlayTime()
    {
        _playTime = LevelManager.Instance.GetCurrentLevelData().PlayTime;
        _playTimeText.text = _playTime.ToString();
    }
    public void OnEnvironmentLoaded(EnvironmentData environmentData)
    {
        LevelManager.Instance.ActiveEnvData = environmentData;
        _hidingSpotAssigner.SetHideSpotList(environmentData.hidingSpots);
        CameraManager.Instance.SetHunterCameraPos(environmentData.hunterCamPos.position);
        _hunter.transform.position = environmentData.hunterSpawnPos.position;   
        _hunter.transform.localScale = environmentData.hunterScale;
        _hunterController.SetCatchDistance(environmentData.hunterCatchDistance);
        _characterCreatePivot = environmentData.charSpawnPos;
        CameraManager.Instance.SetSelectionFollowAndLookAt(_characterCreatePivot);
        GetCurrentPlayTime();
        ResetGamePlay();
        EventManager.RefrencesSet();
    }

    public void Shuffle(List<HideController> values)
    {
        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = UnityEngine.Random.Range(0, i + 1);
            HideController value = values[k];
            values[k] = values[i];
            values[i] = value;
        }
    }
}

