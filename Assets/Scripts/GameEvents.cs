using UnityEngine;
using System;

public static class GameEvents
{
    public static event Action OnGameOver;
    public static event Action OnGameWin;

    public static void RaiseGameOver() => OnGameOver?.Invoke();
    public static void RaiseGameWin() => OnGameWin?.Invoke();
}
