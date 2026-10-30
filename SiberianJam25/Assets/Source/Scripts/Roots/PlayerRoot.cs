using IcePiecesGames.Framework.Core.Events;
using IcePiecesGames.Framework.Core.Lifecycle;
using System;
using UnityEngine;

[DependsOn(typeof(InputRoot))]
[DependsOn(typeof(EventBusRoot))]
public class PlayerRoot : FrameworkRoot, IComposable,IRunnable,IShutdownable
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInterface _interface;

    private InputRoot _inputRoot;

    private IEventBus _eventBus;
    private IDisposable _openTerminalSubscription;
    private IDisposable _closeTerminalSubscription;

    public void Compose()
    {
        _inputRoot = Context.GetRoot<InputRoot>();
        _eventBus = Context.GetService<IEventBus>();

        _player.Initialize(this, _inputRoot, _eventBus);
        _interface.Initialize(this, _eventBus);
    }

    public void Run()
    {
        SubscribeToEvents();

        _player.ToggleActivation(true);
        _interface.ToggleVisibility(true);
    }

    #region >>> PLAYER

    public void TogglePlayerActivation(bool value)
    {
        _player.ToggleActivation(value);
    }
    #endregion
    #region >>> INTERFACE

    private void ToggleInterfaceVisual(bool value)
    {
        _interface.ToggleVisibility(value);
    }

    #endregion
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
        TogglePlayerActivation(false);
        ToggleInterfaceVisual(false);
    }

    private void OnTerminalClosed(TerminalCloseEvent eventData)
    {
        TogglePlayerActivation(true);
        ToggleInterfaceVisual(true);
    }

    #endregion

    public void Shutdown()
    {
        UnSubscribeToEvents();

        _player.Dispose();
        _interface.Dispose();
    }
}

public readonly struct InteractiorDetectedEvent
{
    public readonly string InteractorInfo;

    public InteractiorDetectedEvent(string interactorInfo)
    {
        InteractorInfo = interactorInfo;
    }
}

public readonly struct InteractorLostEvent
{

}
