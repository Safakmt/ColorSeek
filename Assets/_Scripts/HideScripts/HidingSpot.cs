using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] private Transform stickingPoint;
    [SerializeField] private Color _hidingColor;
    [SerializeField] private PoseType _type;
    private HideController currentHider;
    private bool _isFree = true;
    public bool canPlayerHide = true;
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

    public Transform GetHidingTransform()
    {
        return stickingPoint;
    }
    public void SetHidingColor(Color newColor)
    {
        _hidingColor = newColor;
    }
    public Color GetHidingColor()
    {
        return _hidingColor;
    }
    public Vector3 GetHidingPosition()
    {
        return stickingPoint.position;
    }
    public Vector3 GetHidingRotation()
    {
        return stickingPoint.eulerAngles;
    }
    public PoseType GetPose()
    {
        return _type;
    }
    //private void OnDrawGizmos()
    //{
    //    Debug.DrawLine(transform.position, GetHidingPosition());
    //}
}
