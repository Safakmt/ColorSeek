using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HidingSpotAssigner : MonoBehaviour
{
    [SerializeField] private List<HideController> _hiders = new List<HideController>();
    [SerializeField] private List<HidingSpot> _hidingSpotList = new List<HidingSpot>();

    private List<HidingSpot> usedSpots= new List<HidingSpot>();

    public void SetHideSpotList(List<HidingSpot> hidingSpotList)
    {
        _hidingSpotList = hidingSpotList;
        usedSpots.Clear();
        SetNewPositions();
    }

    private void SetNewPositions()
    {
        Debug.Log(LevelManager.Instance.CurrentEnvironment);
        if (LevelManager.Instance.CurrentEnvironment == Environment.tutorial )
        {
            foreach (var item in _hiders)
            {

                if (!item.GetComponent<PlayerController>())
                {
                    item.gameObject.SetActive(false);
                }
            }
            foreach (var item in _hiders)
            {
                if (!item.GetComponent<PlayerController>())
                {
                    item.gameObject.SetActive(true);
                    break;
                }
            }

            foreach (var item in _hiders)
            {
                if (item.gameObject.activeSelf)
                {
                    HidingSpot Spot = _hidingSpotList.First();
                    _hidingSpotList.Remove(Spot);
                    usedSpots.Add(Spot);
                    item.SetRightSpot(Spot);
                }
            }
        }
        else
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
    }

    public List<HideController> GetHideControllers() {
        return _hiders;
    }
}
