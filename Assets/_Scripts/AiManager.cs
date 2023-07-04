using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    [SerializeField] private List<Transform> stickyTransforms = new List<Transform>();
    private AiMovementController[] _aiMovementControllers;
    private List<Transform> usedTransforms= new List<Transform>();
    private void Start()
    {
        _aiMovementControllers = FindObjectsOfType<AiMovementController>();
        SetNewPositionsForAIs();
    }

    private void SetNewPositionsForAIs()
    {
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
