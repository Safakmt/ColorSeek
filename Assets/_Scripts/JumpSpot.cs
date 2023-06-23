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

        if (other.TryGetComponent<AiMovementController>(out AiMovementController aiController))
        {
            aiController.ToggleNavmesh(false);
            Quaternion lookRot = Quaternion.LookRotation(other.transform.position, endPos.position);
            other.transform.DORotate(lookRot.eulerAngles, 1);
            other.transform.DOJump(endPos.position, 2, 1, 1).OnComplete(() =>
            {
                aiController.ToggleNavmesh(true);
            });
        }
    }
}
