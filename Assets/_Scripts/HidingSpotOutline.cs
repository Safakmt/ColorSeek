using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpotOutline : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private float _maxOutlineWidth;
    [SerializeField] private float _highlightDuration;
    private Tween tween;

    private void OnEnable()
    {
        EventManager.OnGameStart += StartHighlight;
        EventManager.OnPlayerUnhide += StartHighlight;
        EventManager.OnPlayerHide += StopHighLight;
    }
    private void OnDisable()
    {
        EventManager.OnGameStart -= StartHighlight;
        EventManager.OnPlayerUnhide -= StartHighlight;
        EventManager.OnPlayerHide -= StopHighLight;
    }
    private void Start()
    {
        _material.SetFloat("_Outline", 0);
        tween = _material.DOFloat(_maxOutlineWidth, "_Outline", _highlightDuration).SetLoops(-1,LoopType.Yoyo);
    }
    private void StopHighLight()
    {
        _material.SetFloat("_Outline", 0);
        tween.Pause();
    }
    private void StartHighlight()
    {
        tween.Play();
    }
}
