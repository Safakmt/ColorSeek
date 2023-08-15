using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoboticArmCatch : MonoBehaviour
{
    [SerializeField] private GameObject _rightHand;
    [SerializeField] private GameObject _leftHand;
    [SerializeField] private GameObject _standingBanana;
    [SerializeField] private GameObject _catchBanana;
    private float _leftHandPos;
    private float _rightHandPos;
    
    private void Start()
    {
        _leftHandPos = _leftHand.transform.localPosition.z;
        _rightHandPos = _rightHand.transform.localPosition.z;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _leftHand.transform.DOLocalMoveZ(-7f, 0.4f).SetEase(Ease.InBack).OnComplete(()=>
            {
                _leftHand.transform.DOLocalMoveZ(_leftHandPos, 0.3f);
            });
        }
        if (Input.GetMouseButtonDown(1))
        {
            _rightHand.transform.DOLocalMoveZ(-7f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _catchBanana.SetActive(true);
                _standingBanana.SetActive(false);
                _rightHand.transform.DOLocalMoveZ(_rightHandPos, 0.3f);
            });
        }
    }
}
