using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    public void RestartButton()
    {
        LevelManager.Instance.ReloadLevel();
    }
}
