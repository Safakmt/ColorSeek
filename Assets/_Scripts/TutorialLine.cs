using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private List<Transform> _tutorialSpots;
    [SerializeField] private GameObject _player;
    private bool _isTutorialStart = false;
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
        if (_isTutorialStart)
        {
            _lineRenderer.SetPosition(_tutorialSpots.Count, _player.transform.position);
        }

    }
    public void PointReached(TutorialPoint tutPoint)
    {
        if (_tutorialSpots[_tutorialSpots.Count-1].GetComponent<TutorialPoint>() == tutPoint && _tutorialSpots.Count != 1)
        {
            _tutorialSpots.RemoveAt(_tutorialSpots.Count - 1);
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
        _isTutorialStart = true;
    }
}
