using System;
using UnityEditor;

public static class EventManager 
{
    public static event Action OnGameStart;
    public static void GameStart() => OnGameStart?.Invoke();

    public static event Action OnPlayerHide;
    public static void PlayerHide() => OnPlayerHide?.Invoke();

    public static event Action OnSeekState;
    public static void SeekState() => OnSeekState?.Invoke();

    public static event Action OnHunterCatch;
    public static void HunterCatch() => OnHunterCatch?.Invoke();

    public static event Action OnHunterScream;
    public static void HunterScream()=> OnHunterScream?.Invoke();

    public static event Action OnPlayerUnhide;
    public static void PlayerUnhide() => OnPlayerUnhide?.Invoke();

    public static event Action OnSceneLoad;
    public static void SceneLoad() => OnSceneLoad?.Invoke();

    public static event Action OnSceneUnload;
    public static void SceneUnload() => OnSceneUnload?.Invoke();

    public static event Action OnRefrencesSet;
    public static void RefrencesSet() => OnRefrencesSet?.Invoke();

    public static event Action<EnvironmentData> OnEnvironmentInitalized;
    public static void EnvironmentInitialized(EnvironmentData environmentData) => OnEnvironmentInitalized?.Invoke(environmentData);

    public static event Action<bool> OnHuntingFinished;
    public static void HuntingFinished(bool isPlayerCatch) => OnHuntingFinished?.Invoke(isPlayerCatch);
}
