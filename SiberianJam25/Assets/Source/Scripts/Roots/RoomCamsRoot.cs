using IcePiecesGames.Framework.Core.Events;
using IcePiecesGames.Framework.Core.Lifecycle;
using System;
using UnityEngine;

[DependsOn(typeof(EventBusRoot))]
public class RoomCamsRoot : FrameworkRoot, IComposable,IRunnable, IShutdownable
{
    [SerializeField] private CCTVController _cctvController;

    private IEventBus _eventBus;
    private IDisposable _openTerminalSubscription;
    private IDisposable _closeTerminalSubscription;

    public void Compose()
    {
        _cctvController.Initialize();

        _eventBus = Context.GetService<IEventBus>();
    }

    public void Run()
    {
        SubscribeToEvents();
    }    

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        if (_openTerminalSubscription != null || _closeTerminalSubscription != null)
            return;

        _openTerminalSubscription = _eventBus.Subscribe<TerminalOpenEvent>(OnTerminalOpend);
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
        _cctvController.Activate();
    }

    private void OnTerminalClosed(TerminalCloseEvent eventData)
    {
        _cctvController.Deactivate();        
    }

    #endregion

    public void Shutdown()
    {
        _cctvController.Deactivate();

        UnSubscribeToEvents();
    }
}
