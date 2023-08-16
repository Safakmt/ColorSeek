using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class RoboticArmCatch : MonoBehaviour
{
    [SerializeField] private GameObject _rightHand;
    [SerializeField] private GameObject _leftHand;
    [SerializeField] private List<CatchableObjectScript> catchingObjects = new List<CatchableObjectScript>();
    [SerializeField] private List<CatchableObjectScript> leftHoldingObjects = new List<CatchableObjectScript>();
    [SerializeField] private List<CatchableObjectScript> rightHoldingObjects = new List<CatchableObjectScript>();
    [SerializeField] private Transform _castAim;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Camera _cam;
    private float _leftHandPos;
    private float _rightHandPos;
    private RaycastHit hit;
    private bool isHit;
    private void Start()
    {
        _leftHandPos = _leftHand.transform.localPosition.z;
        _rightHandPos = _rightHand.transform.localPosition.z;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isHit = Physics.BoxCast(transform.position, Vector3.one,transform.forward, out hit, transform.rotation,_mask);

            _leftHand.transform.DOLocalMoveZ(-7f, 0.4f).SetEase(Ease.InBack).OnComplete(()=>
            {
                if (isHit)
                {
                    if (hit.transform.TryGetComponent<CatchableObjectScript>(out CatchableObjectScript catchScript))
                    {
                        for (int i = 0; i < leftHoldingObjects.Count; i++)
                        {
                            if (leftHoldingObjects[i].objType == catchScript.objType)
                            {
                                Transform tf = leftHoldingObjects[i].transform;
                                leftHoldingObjects[i].gameObject.SetActive(true);
                                catchScript.gameObject.SetActive(false);
                                DOVirtual.DelayedCall(1f, () =>
                                {
                                    tf.DOScale(0, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
                                    {
                                        tf.transform.localScale = leftHoldingObjects[i].initScale;
                                        tf.gameObject.SetActive(false);
                                    });
                                });
                            }
                        }
                    }
                }
                _leftHand.transform.DOLocalMoveZ(_leftHandPos, 0.3f);
            });
        }
        if (Input.GetMouseButtonDown(1))
        {
            isHit = Physics.BoxCast(transform.position, Vector3.one, transform.forward, out hit, transform.rotation, _mask);

            _rightHand.transform.DOLocalMoveZ(-7f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
            {
                if (isHit)
                {
                    if (hit.transform.TryGetComponent<CatchableObjectScript>(out CatchableObjectScript catchScript))
                    {
                        for (int i = 0; i < rightHoldingObjects.Count; i++)
                        {
                            if (rightHoldingObjects[i].objType == catchScript.objType)
                            {
                                Transform tf = rightHoldingObjects[i].transform;
                                rightHoldingObjects[i].gameObject.SetActive(true);
                                catchScript.gameObject.SetActive(false);
                                DOVirtual.DelayedCall(1f, () =>
                                {
                                    tf.DOScale(0, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
                                    {
                                        tf.transform.localScale = rightHoldingObjects[i].initScale;
                                        tf.gameObject.SetActive(false);
                                    });
                                });
                            }
                        }
                    }
                }
                _rightHand.transform.DOLocalMoveZ(_rightHandPos, 0.3f);
            });
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (isHit)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * 100f);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * 100f, transform.localScale);
        }
    }
}
