using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPoint : MonoBehaviour
{
    [SerializeField] private TutorialLine _tutLine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _tutLine.PointReached(this);
        }
    }
}
