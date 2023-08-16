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
    private float _leftHandPos;
    private float _rightHandPos;
    
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
            _leftHand.transform.DOLocalMoveZ(-7f, 0.4f).SetEase(Ease.InBack).OnComplete(()=>
            {
                if (catchingObjects.Count != 0)
                {
                    var obj = catchingObjects.First();
                    for (int i = 0; i < leftHoldingObjects.Count; i++)
                    {
                        if (leftHoldingObjects[i].objType == obj.objType)
                        {
                            Transform tf = leftHoldingObjects[i].transform;
                            leftHoldingObjects[i].gameObject.SetActive(true);
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
                    obj.gameObject.SetActive(false);
                    catchingObjects.Remove(obj);
                }
                _leftHand.transform.DOLocalMoveZ(_leftHandPos, 0.3f);
            });
        }
        if (Input.GetMouseButtonDown(1))
        {
            _rightHand.transform.DOLocalMoveZ(-7f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
            {
                if (catchingObjects.Count != 0)
                {
                    var obj = catchingObjects.First();
                    for (int i = 0; i < rightHoldingObjects.Count; i++)
                    {
                        if (rightHoldingObjects[i].objType == obj.objType)
                        {
                            Transform tf = rightHoldingObjects[i].transform;
                            rightHoldingObjects[i].gameObject.SetActive(true);
                            DOVirtual.DelayedCall(1f, () =>
                            {
                                tf.transform.DOScale(0, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
                                {
                                    tf.gameObject.SetActive(false);
                                    tf.transform.localScale = leftHoldingObjects[i].initScale;
                                });
                            });
                        }
                    }
                    obj.gameObject.SetActive(false);
                    catchingObjects.Remove(obj);
                }
                _rightHand.transform.DOLocalMoveZ(_rightHandPos, 0.3f);
            });
        }
    }
}
