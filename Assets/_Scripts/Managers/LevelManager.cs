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
                AsyncOperation asyncOp = SceneManager.LoadSceneAsync("Kitchen", LoadSceneMode.Additive);
                StartCoroutine(WaitForSceneLoad(asyncOp,Environment.kitchen));
                break;
            case Environment.childroom:
                break;
            case Environment.table:
                break;
            case Environment.tutorial:
                break;
        }
    }

    IEnumerator WaitForSceneLoad(AsyncOperation op, Environment env)
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
