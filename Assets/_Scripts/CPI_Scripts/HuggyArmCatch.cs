using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class HuggyArmCatch : MonoBehaviour
{
    [SerializeField] private List<CatchableObjectScript> rightHoldingObjects = new List<CatchableObjectScript>();
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Camera _mainCam;
    private float _downParameter = -0.2f;
    private RaycastHit hit;
    private bool isHit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RightHandCatch();
        }
    }

    private void RightHandCatch()
    {
        isHit = Physics.BoxCast(_mainCam.transform.position, Vector3.one * 0.5f, _mainCam.transform.forward + new Vector3(0, _downParameter, 0), out hit, transform.rotation, _mask);

        if (isHit)
        {
            if (hit.transform.TryGetComponent<CatchableObjectScript>(out CatchableObjectScript catchScript))
            {
                CatchableObjectScript holdingScript = null;
                _animator.SetTrigger("Attack");
                for (int i = 0; i < rightHoldingObjects.Count; i++)
                {
                    if (rightHoldingObjects[i].objType == catchScript.objType)
                    {
                        holdingScript = rightHoldingObjects[i];
                    }
                }
                DOVirtual.DelayedCall(0.40f, () =>
                {
                    holdingScript.gameObject.SetActive(true);
                    catchScript.gameObject.SetActive(false);
                    DOVirtual.DelayedCall(1.4f, () =>
                    {
                        holdingScript.transform.DOScale(0, 0.4f)
                        .SetEase(Ease.InBack)
                        .OnComplete(() =>
                        {
                            holdingScript.transform.localScale = holdingScript.initScale;
                            holdingScript.gameObject.SetActive(false);
                        });
                    });
                });

            }

        }
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (isHit)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(_mainCam.transform.position, (_mainCam.transform.forward + new Vector3(0, _downParameter, 0)) * hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(_mainCam.transform.position + (_mainCam.transform.forward + new Vector3(0, _downParameter, 0)) * hit.distance, Vector3.one * 0.5f);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(_mainCam.transform.position, (_mainCam.transform.forward + new Vector3(0, _downParameter, 0)) * 100f);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(_mainCam.transform.position + (_mainCam.transform.forward + new Vector3(0, _downParameter, 0)) * 100f, Vector3.one * 0.5f);
        }
    }
}
