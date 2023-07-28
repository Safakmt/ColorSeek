using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideController : MonoBehaviour
{
    [SerializeField] private HidingSpot _closestSpot;
    [SerializeField] private MeshColorHandler _colorHandler;
    [SerializeField] private AnimatorController _animController;
    [SerializeField] private HidingSpot _rightSpot;
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

    public void Hide()
    {
        transform.SetPositionAndRotation(_closestSpot.GetHidingPosition(), Quaternion.Euler(_closestSpot.GetHidingRotation()));
        _colorHandler.HidingValues();
        _closestSpot.SetCurrentHider(this);
        IsHiding = true;
    }

    public void Unhide()
    {
        _colorHandler.UnhidingValues();
        if (_closestSpot)
            _closestSpot.ClearCurrentHider();
        IsHiding = false;
    }
}
