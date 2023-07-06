using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private HideController hideController;
    private const string _validLayer = "Hideable";
    private int _layerIndex;
    private void Awake()
    {
        _layerIndex = LayerMask.NameToLayer(_validLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerIndex)
        {
            HidingSpot currentSpot = other.GetComponent<HidingSpot>();
            hideController.SetHidingSpot(currentSpot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _layerIndex)
        {
            hideController.ClearHidingSpot();
        }
    }
}
