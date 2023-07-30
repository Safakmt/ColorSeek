using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class JumpSpot : MonoBehaviour
{
    [SerializeField] private Transform startPos;    
    [SerializeField] private Transform endPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.StopTakingInput();
            controller.TriggerJumpAnim();
            other.transform.DOJump(startPos.position, 1, 1, 0.5f).OnComplete(() =>
            {
                other.transform.DOJump(endPos.position, 1.5f, 1, 1.5f).SetEase(Ease.OutBack);
            });
            DOVirtual.DelayedCall(1.7f, () =>
            {
                other.transform.DOKill();
                controller.StartTakingInputs();
            });
        }

        if (other.TryGetComponent<AiMovementController>(out AiMovementController aiController))
        {
            
            aiController.ToggleAgent(false);
            Quaternion lookRot = Quaternion.LookRotation(aiController.transform.position, endPos.position);
            lookRot.eulerAngles = new Vector3(0,lookRot.y,0);
            aiController.transform.DORotate(lookRot.eulerAngles, 1);
            aiController.transform.DOJump(endPos.position, 2, 1, 1).OnComplete(() =>
            {
                aiController.ToggleAgent(true);
            });
        }
    }
}
