using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideController : MonoBehaviour
{
    [SerializeField] private HidingSpot _closestSpot;
    [SerializeField] private MeshColorHandler _colorHandler;
    [SerializeField] private HidingSpot _rightSpot;
    public void SetClosestSpot(HidingSpot spot)
    {
        _closestSpot = spot;
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
        this._closestSpot = null;
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
    }

    public void Unhide()
    {
        _colorHandler.UnhidingValues();
        _closestSpot.ClearCurrentHider();
    }
}
