using System;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    public bool IsCompleted;
    public event Action Completed;

    public abstract void Initialize(MiniGamesService controller);

    public abstract void StartGame();

    public abstract void PauseGame();

    protected virtual void CompleteGame()
    {
        if (IsCompleted)
            return;
        IsCompleted = true;
        Completed?.Invoke();
    }
}
