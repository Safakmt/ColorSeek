using System;


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
}
