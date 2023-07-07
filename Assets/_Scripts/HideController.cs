using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideController : MonoBehaviour
{
    [SerializeField] private HidingSpot hidingSpot;

    public void SetHidingSpot(HidingSpot spot)
    {
        hidingSpot = spot;
    }
    public void ClearHidingSpot()
    {
        this.hidingSpot = null;
    }
    public bool IsReadyToHide()
    {
        if (hidingSpot != null)
            return hidingSpot.IsFreeToHide();
        return false;

    }

    public void Hide()
    {
        transform.position = hidingSpot.GetHidingPosition();
        hidingSpot.SetCurrentHider(this);
    }

    public void Unhide()
    {
        hidingSpot.ClearCurrentHider();
    }
}
