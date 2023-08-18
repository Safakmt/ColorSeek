using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CatchableObjectType
{
    Banana,
    Chilipepper,
    Pepper,
    Eggplant,
    Tomato,
    Throphy1,
    Throphy2,
    ThropyStickman,
    StickmanWhite,
    StickmanGrey,
    StickmanPurple,
    StickmanRed,
    StickmanLightGrey,
    StickmanBlue,
    StickmanYellow
}
public class CatchableObjectScript : MonoBehaviour
{
    public CatchableObjectType objType;
    public Vector3 initScale;

    private void Start()
    {
        initScale = transform.localScale;
    }
}
