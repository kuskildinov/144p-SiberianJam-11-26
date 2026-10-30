using IcePiecesGames.Framework.Core.Lifecycle;
using IcePiecesGames.Framework.Core.Events;
using UnityEngine;
using Unity.Cinemachine;

[DependsOn(typeof(InputRoot))]
[DependsOn(typeof(RoomRoot))]
[DependsOn(typeof(EventBusRoot))]
public class TerminalRoot : FrameworkRoot, IComposable, IRunnable, IShutdownable
{    
    [Header("Terminal")]
    [SerializeField] private ComputerTerminal _terminal;
    [SerializeField] private CinemachineCamera _terminalCam;
    [SerializeField] private TerminalStartView _startView;
    [SerializeField] private MiniGameVictoryView _victoryView;
    [SerializeField] private TerminalCameraView _cameraView;

    private InputRoot _inputRoot;
    private RoomRoot _rootRoot;
    private IEventBus _eventBus;
    private MiniGamesService _miniGamesService;

    private bool _isOpened;
    private bool _miniGameStarted;

    public bool IsOpened => _isOpened;
    public string DoorCode => _rootRoot.DoorCode;

    public void Compose()
    {
        _inputRoot = Context.GetRoot<InputRoot>();
        _rootRoot = Context.GetRoot<RoomRoot>();
        _miniGamesService = new MiniGamesService();
        Context.RegisterService<MiniGamesService>(_miniGamesService);

        _eventBus = Context.GetService<IEventBus>();
               
        _terminal.Initialize(this);
        _startView.Initialize(this);
        _victoryView.Initialize(this);
        _cameraView.Initialize();
    }

    public void Run()
    {
        _startView.Open();

        SubscribeToEvents();
    }    

    #region >>> TERMINAL

    public void OpenTerminal()
    {
        if (_isOpened)
            return;

        _isOpened = true;
        ToggleTerminalCam(true);

        _eventBus.Publish(new TerminalOpenEvent());

        if (!_miniGameStarted)
            _startView.Open();       
    }

    public void CloseTerminal()
    {
        if (!_isOpened)
            return;

        _isOpened = false;
        ToggleTerminalCam(false);
        _cameraView.Close();
        _eventBus.Publish(new TerminalCloseEvent());
    } 

    private void ToggleTerminalCam(bool value)
    {
        _terminalCam.Priority = value ? 20 : 0;
    }

    public void TryStartMiniGame()
    {
        if (_miniGameStarted)
            return;

        _miniGameStarted = true;
        _startView.Close();
        _miniGamesService.StartGame();
    }

    public void OnContinueClicked()
    {       
        CloseTerminal();
    }

    private void OnGameCompleted()
    {
        _victoryView.Open();
        _eventBus.Publish(new GameCompleteEvent());
    }

    #endregion    
    #region >>> EVETNS

    private void SubscribeToEvents()
    {
        _inputRoot.TerminalExited += CloseTerminal;       
        _miniGamesService.Completed += OnGameCompleted;
    }

    private void UnsubscribeToEvents()
    {
        if (_inputRoot != null)
            _inputRoot.TerminalExited -= CloseTerminal;

        _miniGamesService.Completed -= OnGameCompleted;
    }

    private void OnPlayerCaught()
    {
        CloseTerminal();
    }

    #endregion

    public void Shutdown()
    {
        UnsubscribeToEvents();
        ToggleTerminalCam(false);

        _isOpened = false;

        _victoryView.Dispose();
        _miniGamesService.Dispose();
        _startView.Dispose();

    }
}

public readonly struct TerminalOpenEvent
{

}

public readonly struct TerminalCloseEvent
{

}

public readonly struct GameCompleteEvent
{

}
