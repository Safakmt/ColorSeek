using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class JumpSpot : MonoBehaviour
{
    [SerializeField] private Transform startPos;    
    [SerializeField] private Transform endPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            other.transform.DOJump(endPos.position, 2, 1, 1);
        }
    }
}
