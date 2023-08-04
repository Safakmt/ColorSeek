using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HideController : MonoBehaviour
{
    [SerializeField] private HidingSpot _closestSpot;
    [SerializeField] private MeshColorHandler _colorHandler;
    [SerializeField] private AnimatorController _animController;
    [SerializeField] private HidingSpot _rightSpot;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private GameObject _selectionHighlight;
    public bool IsHiding { get; set; }
    public void SetClosestSpot(HidingSpot spot)
    {
        if (!IsHiding)
        {
            _closestSpot = spot;
        }
    }
    public void SetRightSpot(HidingSpot spot)
    {
        _rightSpot = spot;
        _colorHandler.SetColor(_rightSpot.GetHidingColor());
    }
    public Transform GetRightSpot()
    {
        return _rightSpot.GetHidingTransform();
    }
    public void ClearClosestSpot()
    {
        if (!IsHiding)
        {
            _closestSpot = null;
        }
    }
    public bool IsReadyToHide()
    {
        if (_closestSpot != null)
            return _closestSpot.IsFreeToHide();
        return false;

    }
    public bool IsHidingRightSpot()
    {

        if (_closestSpot.GetHidingColor() == _rightSpot.GetHidingColor())
        {
            Debug.Log(true);
            return true;
        }
        else
        {
            Debug.Log(false);
            return false;

        }
    }
    public void Hide()
    {
        //transform.SetPositionAndRotation(_closestSpot.GetHidingPosition(), Quaternion.Euler(_closestSpot.GetHidingRotation()));
        transform.DOJump(_closestSpot.GetHidingPosition(), 0.5f, 1, 0.5f);
        transform.DORotate(_closestSpot.GetHidingRotation(), 0.5f);
        _colorHandler.HidingValues();
        _closestSpot.SetCurrentHider(this);
        IsHiding = true;
    }

    public void Unhide()
    {
        transform.DOKill();
        _colorHandler.UnhidingValues();
        if (_closestSpot)
            _closestSpot.ClearCurrentHider();
        IsHiding = false;
    }

    public TextMeshProUGUI GetName()
    {
        return _name;
    }

    public void ToggleSelection(bool value)
    {
        _selectionHighlight.SetActive(value);
    }
}
