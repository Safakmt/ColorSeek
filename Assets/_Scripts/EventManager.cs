using System;


public static class EventManager 
{
    public static event Action OnGameStart;
    public static void GameStart() => OnGameStart?.Invoke();

    public static event Action OnPlayerHide;
    public static void PlayerHide() => OnPlayerHide?.Invoke();

    public static event Action OnTriggerExit;
}
