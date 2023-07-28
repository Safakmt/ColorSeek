using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class HidingSpotAssigner : MonoBehaviour
{
    [SerializeField] private List<HidingSpot> _hidingSpotList = new List<HidingSpot>();
    private List<HidingSpot> usedSpots= new List<HidingSpot>();
    private HideController[] _hiders;

    private void Start()
    {
        SearchForHiders();
    }

    public void SetHideSpotList(List<HidingSpot> hidingSpotList)
    {
        _hidingSpotList = hidingSpotList;
        usedSpots.Clear();
        SetNewPositions();
    }

    private void SetNewPositions()
    {

        foreach (var item in _hiders)
        {
            int randomInt = Random.Range(0, _hidingSpotList.Count);
            HidingSpot Spot = _hidingSpotList[randomInt];
            _hidingSpotList.Remove(Spot);
            usedSpots.Add(Spot);

            item.SetRightSpot(Spot);
        }

    }

    public void SearchForHiders()
    {
        _hiders = FindObjectsOfType<HideController>();
    }

    public HideController[] GetHideControllers() {
        return _hiders;
    }
}
