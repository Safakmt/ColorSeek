using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] private Transform stickingPoint;
    private HideController currentHider;
    private bool _isFree = true;

    public void SetCurrentHider(HideController currentHider)
    {
        _isFree = false;
        this.currentHider = currentHider;
    }

    public void ClearCurrentHider()
    {
        this.currentHider = null;
        _isFree = true;
    }
    public bool IsFreeToHide()
    {
        return _isFree;
    }

    public Vector3 GetHidingSpot()
    {
        return stickingPoint.position;
    }
}
