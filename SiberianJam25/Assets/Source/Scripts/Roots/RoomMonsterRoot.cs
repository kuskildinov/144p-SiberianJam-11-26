using IcePiecesGames.Framework.Core.Events;
using IcePiecesGames.Framework.Core.Lifecycle;
using System;
using UnityEngine;

[DependsOn(typeof(EventBusRoot))]
public class RoomMonsterRoot : FrameworkRoot, IComposable, IRunnable, IShutdownable
{
    [SerializeField] private MonsterController _monster;

    private IEventBus _eventBus;
    private IDisposable _openTerminalSubscription;
    private IDisposable _closeTerminalSubscription;

    public void Compose()
    {
        _monster.Initialize(this);

        _eventBus = Context.GetService<IEventBus>();
    }

    public void Run()
    {
        SubscribeToEvents();
    }

    public void OnPlayerCaught()
    {
        _eventBus.Publish(new PlayerCaughtEvent());
    }    

    #region >>> EVENTS

    private void SubscribeToEvents()
    {       
        if (_openTerminalSubscription == null)
            _openTerminalSubscription = _eventBus.Subscribe<TerminalOpenEvent>(OnTerminalOpend);
        if (_closeTerminalSubscription == null) 
            _closeTerminalSubscription = _eventBus.Subscribe<TerminalCloseEvent>(OnTerminalClosed);
    }

    private void UnSubscribeToEvents()
    {
        _openTerminalSubscription?.Dispose();
        _openTerminalSubscription = null;

        _closeTerminalSubscription?.Dispose();
        _closeTerminalSubscription = null;
    }

    private void OnTerminalOpend(TerminalOpenEvent eventData)
    {
        _monster.StartMoving();
    }

    private void OnTerminalClosed(TerminalCloseEvent eventData)
    {
        _monster.StopMoving();
    }    

    #endregion

    public void Shutdown()
    {
        _monster.ResetMonster();

        UnSubscribeToEvents();
    }
}

public readonly struct PlayerCaughtEvent
{

}
