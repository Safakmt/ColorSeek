using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Environment
{
    kitchen,
    childroom,
    table,
    tutorial,
    chineseshop
}
public class EnvironmentData : MonoBehaviour
{
    public Transform charSpawnPos;
    public Transform hunterSpawnPos;
    public Transform hunterCamPos;
    public Environment environment;
    public List<HidingSpot> hidingSpots;
    public Vector3 hunterScale;
    public float hunterCatchDistance;
    private void Awake()
    {
        EventManager.EnvironmentInitialized(this);
        Debug.Log(environment.ToString() + " Loaded");
    }

}