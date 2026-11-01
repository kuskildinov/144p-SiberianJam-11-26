using IcePiecesGames.Framework.Core.Lifecycle;
using IcePiecesGames.Framework.Core.Events;
using UnityEngine;
using Unity.Cinemachine;
using IcePiecesGames.Framework.Audio;

[DependsOn(typeof(InputRoot))]
[DependsOn(typeof(RoomRoot))]
[DependsOn(typeof(EventBusRoot))]
[DependsOn(typeof(AudioRoot))]
public class TerminalRoot : FrameworkRoot, IComposable, IRunnable, IShutdownable
{    
    [Header("Terminal")]
    [SerializeField] private ComputerTerminal _terminal;
    [SerializeField] private CinemachineCamera _terminalCam;
    [SerializeField] private TerminalStartView _startView;
    [SerializeField] private MiniGameVictoryView _victoryView;
    [SerializeField] private TerminalCameraView _cameraView;
    [Header("Sounds")]
    [SerializeField] private AudioClip _openTerminalSound;
    [SerializeField] private AudioClip _terminalButtonSound;
    [SerializeField] private AudioClip _winGameSound;
    [SerializeField] private AudioClip _incorrectSound;
    [SerializeField] private AudioClip _flipCardSound;

    private InputRoot _inputRoot;
    private RoomRoot _rootRoot;

    private IEventBus _eventBus;
    private IAudioService _audioService;
    private MiniGamesService _miniGamesService;

    private bool _isOpened;
    private bool _miniGameStarted;

    public bool IsOpened => _isOpened;
    public string DoorCode => _rootRoot.DoorCode;

    public void Compose()
    {
        _inputRoot = Context.GetRoot<InputRoot>();
        _rootRoot = Context.GetRoot<RoomRoot>();
        _miniGamesService = new MiniGamesService(this);
        Context.RegisterService<MiniGamesService>(_miniGamesService);

        _eventBus = Context.GetService<IEventBus>();
        _audioService = Context.GetService<IAudioService>();

        _terminal.Initialize(this);
        _startView.Initialize(this);
        _victoryView.Initialize(this);
        _cameraView.Initialize(this);
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
        PlayOpenTerminalSound();

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
        PlayWinGameSound();
        _eventBus.Publish(new GameCompleteEvent());
    }

    #region >>> SOUNDS

    public void PlayOpenTerminalSound()
    {
        _audioService.PlaySfx(_openTerminalSound);
    }

    public void PlayTerminalButtonSound()
    {
        _audioService.PlaySfx(_terminalButtonSound);
    }

    private void PlayWinGameSound()
    {
        _audioService.PlaySfx(_winGameSound);
    }

    public void PlayIncorrectSound()
    {
        _audioService.PlaySfx(_incorrectSound);
    }

    public void PlayFlipCardSound()
    {
        if (_flipCardSound == null)
            return;

        _audioService.PlaySfx(_flipCardSound);
    }

    #endregion

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
