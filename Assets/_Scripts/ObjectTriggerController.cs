using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTriggerController : MonoBehaviour
{
    [SerializeField] private Transform stickingPoint;
    public ISticker currentSticker;
    public bool isFree = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ISticker>(out ISticker sticker) && isFree)
        {
            currentSticker = sticker;
            currentSticker.SetStickPoint(stickingPoint);
            isFree = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ISticker>(out ISticker sticker) && sticker == currentSticker)
        {
            currentSticker.ClearStickPoint();
            isFree = true;
            currentSticker = null;
        }
    }
}
