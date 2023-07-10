using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    [SerializeField] private List<Transform> stickyTransforms = new List<Transform>();
    private AiMovementController[] _aiMovementControllers;
    private HidingSpot[] _hidingSpots;
    private List<Transform> usedTransforms= new List<Transform>();
    private void Start()
    {
        _aiMovementControllers = FindObjectsOfType<AiMovementController>();
        _hidingSpots = FindObjectsOfType<HidingSpot>();
        SetNewPositionsForAIs();
    }

    private void SetNewPositionsForAIs()
    {
        for (int i = 0; i < _hidingSpots.Length; i++)
        {
            stickyTransforms.Add(_hidingSpots[i].transform);
        }
        if (usedTransforms.Count > 0)
        {
            stickyTransforms.AddRange(usedTransforms);
            usedTransforms.Clear();
        }
        foreach (var item in _aiMovementControllers)
        {
            Transform newDestination = stickyTransforms[Random.Range(0, stickyTransforms.Count)];
            stickyTransforms.Remove(newDestination);
            usedTransforms.Add(newDestination);
            item.SetDestination(newDestination);
        }

    }
}
