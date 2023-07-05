using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTriggerController : MonoBehaviour
{
    private const string _validLayer = "Sticker";
    [SerializeField] private Transform stickingPoint;
    private HideController currentHider;
    public bool isFree = true;
    private int _layerIndex;
    private void Awake()
    {
        _layerIndex = LayerMask.NameToLayer(_validLayer);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerIndex)
        {
            if (isFree)
            {
                currentHider = other.GetComponent<HideController>();
                currentHider.SetHidingSpot(stickingPoint);
                isFree = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _layerIndex) { 

            if (other.GetComponent<HideController>() == currentHider)
            {
                currentHider.ClearHidingSpot();
                isFree = true;
                currentHider = null;
            }
        
        }
    }
}
