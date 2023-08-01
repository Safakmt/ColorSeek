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

    public static LevelManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LoadLevel(levelList[currentLevel].Environment);
    }
    public void LoadLevel(Environment env)
    {
        //switch (levelList[currentLevel].Environment)
        //{
        //    case Environment.kitchen:
        //        break;
        //    case Environment.childroom:
        //        StartCoroutine(WaitForSceneLoad(SceneManager.LoadSceneAsync("ChildRoom", LoadSceneMode.Additive)));
        //        break;
        //    case Environment.table:
        //        StartCoroutine(WaitForSceneLoad(SceneManager.LoadSceneAsync("Table", LoadSceneMode.Additive)));
        //        break;
        //    case Environment.tutorial:
        //        StartCoroutine(WaitForSceneLoad(SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive)));
        //        break;
        //}
        if (SceneManager.GetSceneByName(_currentEnvironment.ToString()).isLoaded)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_currentEnvironment.ToString());
            StartCoroutine(WaitForAsyncOp(unloadOp, false));
        }
        _currentEnvironment = env;
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(_currentEnvironment.ToString(), LoadSceneMode.Additive);
        StartCoroutine(WaitForAsyncOp(loadOp, true));

    }
    public LevelDataSO GetCurrentLevelData()
    {
        return levelList[currentLevel];
    }

    public void LoadNextLevel()
    {
        if (currentLevel+1 != levelList.Count)
        {
            currentLevel++;
            LoadLevel(levelList[currentLevel].Environment);
            Debug.Log("loading next level");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadLevel(Environment.childroom);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadLevel(Environment.tutorial);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadLevel(Environment.kitchen);

        }
    }
    IEnumerator WaitForAsyncOp(AsyncOperation op,bool isLoadOperation)
    {
        yield return new WaitUntil(() => op.isDone == true);
        if (isLoadOperation)
            EventManager.SceneLoad();
        else
            EventManager.SceneUnload();
    }
    public void NextLevel()
    {
        if (currentLevel+1 < levelList.Count)
        {
            currentLevel++;
        }
    }
}
