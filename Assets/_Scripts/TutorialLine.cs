using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private List<Transform> _tutorialSpots;
    [SerializeField] private GameObject _player;
    private bool isTutorialStart = false;

    private void OnEnable()
    {
        EventManager.OnGameStart += InitTutorial;

    }
    private void OnDisable()
    {
        EventManager.OnGameStart -= InitTutorial;
    }
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>().gameObject;
    }
    private void Update()
    {
        if (!isTutorialStart)
            return;
        if (_tutorialSpots.Count == 0)
            return;
        _lineRenderer.SetPosition(_tutorialSpots.Count, _player.transform.position);
        if (Vector3.Distance(_player.transform.position, _tutorialSpots[_tutorialSpots.Count-1].position) < 0.5f)
        {
            _tutorialSpots.RemoveAt(_tutorialSpots.Count-1);
            _lineRenderer.positionCount--;
        }
    }

    private void InitTutorial()
    {
        _lineRenderer.positionCount = _tutorialSpots.Count + 1;
        for (int i = 0; i < _tutorialSpots.Count; i++)
        {
            _lineRenderer.SetPosition(i, _tutorialSpots[i].position);
        }
        isTutorialStart = true;
    }
}
