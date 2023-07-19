using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class HidingSpotAssigner : MonoBehaviour
{
    [SerializeField] private List<HidingSpot> _hidingSpotList = new List<HidingSpot>();
    private List<HidingSpot> usedSpots= new List<HidingSpot>();
    private HidingSpot[] _hidingSpots;
    private HideController[] _hiders;

    private void Awake()
    {
        SearchForHiders();
        SearchForHideSpots();
        SetNewPositions();
    }

    private void SetNewPositions()
    {

        if (usedSpots.Count > 0)
        {
            _hidingSpotList.AddRange(usedSpots);
            usedSpots.Clear();
        }
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

    public void SearchForHideSpots()
    {
        _hidingSpots = FindObjectsOfType<HidingSpot>();
        for (int i = 0; i < _hidingSpots.Length; i++)
        {
            _hidingSpotList.Add(_hidingSpots[i]);
        }
    }
}
