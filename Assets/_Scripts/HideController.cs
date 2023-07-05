using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideController : MonoBehaviour
{
    [SerializeField] private Transform HidingSpot;

    public void SetHidingSpot(Transform HidingSpot)
    {
        this.HidingSpot = HidingSpot;
    }
    public void ClearHidingSpot()
    {
        this.HidingSpot = null;
    }
    public bool IsReadyToHide()
    {
        return HidingSpot != null;
    }

    public void Hide()
    {
        transform.position = HidingSpot.position;
    }
}
