using System;

public sealed class MiniGamesService : IDisposable
{
    private readonly MiniGame _currentGame;

    private bool _started;
    private bool _completed;
    private bool _disposed;

    public bool IsCompleted => _completed;

    public event Action Completed;

    public MiniGamesService()
    {
        _currentGame = UnityEngine.Object.FindAnyObjectByType<MiniGame>();

        _currentGame.Initialize(this);
        SubscribeToEvents();
    }

    public void StartGame()
    {
        ThrowIfDisposed();

        if (_started || _completed)
            return;

        _started = true;
        _currentGame.StartGame();
    }

    public void PauseGame()
    {
        if (_disposed || !_started || _completed)
            return;

        _currentGame.PauseGame();
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(MiniGamesService));
    }

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _currentGame.Completed += OnGameCompleted;
    }

    private void UnsubscribeToEvents()
    {
        _currentGame.Completed -= OnGameCompleted;
    }

    private void OnGameCompleted()
    {
        if (_disposed || _completed)
            return;

        _completed = true;
        _started = false;

        Completed?.Invoke();
    }

    #endregion

    public void Dispose()
    {
        if (_disposed)
            return;

        if (_started && !_completed && _currentGame != null)
            _currentGame.PauseGame();

        UnsubscribeToEvents();

        Completed = null;
        _disposed = true;
    }
}
