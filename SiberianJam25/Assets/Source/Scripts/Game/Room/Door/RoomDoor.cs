using IcePiecesGames.Framework.Core.Events;
using System;
using UnityEngine;

public class RoomDoor : MonoBehaviour, IDisposable
{
    [SerializeField] private DoorView _view;
    [SerializeField] private DoorLock _lock;

    private RoomRoot _root;
    private IEventBus _eventBus;
    private IDisposable _gameCompleteSubscription;

    public string DoorCode => _root.DoorCode;

    public void Initialize(RoomRoot root, IEventBus eventBus)
    {
        _root = root;
        _eventBus = eventBus;

        _view.Initilize();
        _lock.Initialize(this);

        SubscribeToEvents();
    }

    public void OnCorrectCodeEntered()
    {
        _root.OnDoorOpend();
    }

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        if (_gameCompleteSubscription == null)
            _gameCompleteSubscription = _eventBus.Subscribe<GameCompleteEvent>(OnGameComplete);
    }

    private void UnsubscribeToEvents()
    {
        _gameCompleteSubscription?.Dispose();
        _gameCompleteSubscription = null;
    }

    private void OnGameComplete(GameCompleteEvent eventData)
    {
        _lock.SetLockState(DoorLock.DoorLockState.Active);
    }

    public void Dispose()
    {
        UnsubscribeToEvents();
    }

    #endregion
}
