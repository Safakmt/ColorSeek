using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    private AiMovementController[] _aiMovementControllers;

    private void Start()
    {
        _aiMovementControllers = FindObjectsOfType<AiMovementController>();
    }

}
