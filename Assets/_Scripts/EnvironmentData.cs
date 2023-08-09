using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Environment
{
    kitchen,
    childroom,
    dinnertable,
    tutorial,
    chineseshop,
    kitchen1,
    childroom1
}
public class EnvironmentData : MonoBehaviour
{
    public Transform charSpawnPos;
    public Transform hunterSpawnPos;
    public Transform hunterCamPos;
    public Transform followCamPos;
    public Environment environment;
    public GameObject Wall;
    public List<HidingSpot> hidingSpots;
    public Vector3 hunterScale;
    public float hunterCatchDistance;
    private void Awake()
    {
        EventManager.EnvironmentInitialized(this);
        Debug.Log(environment.ToString() + " Loaded");
    }

}