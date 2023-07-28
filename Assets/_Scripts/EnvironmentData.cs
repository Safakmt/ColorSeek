using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Environment
{
    kitchen,
    childroom,
    table,
    tutorial
}
public class EnvironmentData : MonoBehaviour
{
    public Transform charSpawnPos;
    public Transform hunterSpawnPos;
    public Transform hunterCamPos;
    public Environment environment;
    public List<HidingSpot> hidingSpots;

    private void Awake()
    {
        EventManager.EnvironmentInitialized(this);
        Debug.Log(environment.ToString() + " Loaded");
    }

}