using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private List<LevelDataSO> levelList;
    public EnvironmentData activeEnvData;
    private Environment _currentEnvironment;
    private void Start()
    {
        LoadLevel();
    }
    public void LoadLevel()
    {
        switch (levelList[currentLevel].Environment)
        {
            case Environment.kitchen:
                StartCoroutine(WaitForSceneLoad(SceneManager.LoadSceneAsync("Kitchen", LoadSceneMode.Additive)));
                break;
            case Environment.childroom:
                StartCoroutine(WaitForSceneLoad(SceneManager.LoadSceneAsync("ChildRoom", LoadSceneMode.Additive)));
                break;
            case Environment.table:
                StartCoroutine(WaitForSceneLoad(SceneManager.LoadSceneAsync("Table", LoadSceneMode.Additive)));
                break;
            case Environment.tutorial:
                StartCoroutine(WaitForSceneLoad(SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive)));
                break;
        }
    }

    IEnumerator WaitForSceneLoad(AsyncOperation op)
    {
        yield return new WaitUntil(() => op.isDone == true);
        EventManager.SceneLoad();
    }

    public void NextLevel()
    {
        if (currentLevel+1 < levelList.Count)
        {
            currentLevel++;
        }
    }
}
