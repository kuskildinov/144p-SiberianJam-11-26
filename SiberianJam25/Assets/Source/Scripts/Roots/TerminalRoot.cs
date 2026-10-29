
using IcePiecesGames.Framework.Core.Lifecycle;
using UnityEngine;

[DependsOn(typeof(InputRoot))]
[DependsOn(typeof(PlayerRoot))]
public class TerminalRoot : FrameworkRoot, IComposable, IRunnable, IShutdownable
{
    [Header("Components")]
    [SerializeField] private CCTVController _cctvController;
    [SerializeField] private GameObject _terminalUI;
    [Header("Terminal")]
    [SerializeField] private ComputerTerminal _terminal;
    [SerializeField] private GameObject _terminalCam;
    [Header("Monster")]
    [SerializeField] private MonsterController _monsterController;

    private InputRoot _inputRoot;
    private PlayerRoot _playerRoot;

    private bool _isOpened;

    public bool IsOpened => _isOpened;

    public void Compose()
    {
        _inputRoot = Context.GetRoot<InputRoot>();
        _playerRoot = Context.GetRoot<PlayerRoot>();

        _cctvController.Initialize();
        _terminal.Initialize(this);
        _monsterController.Initialize();
    }

    public void Run()
    {
        SubscribeToEvents();

        if (_terminalUI != null)
            _terminalUI.SetActive(false);
    }

    public void OpenTerminal()
    {
        if (_isOpened)
            return;

        _isOpened = true;

        _playerRoot.TogglePlayerActivation(false);
        _inputRoot.SetMode(InputRoot.InputMode.Terminal);

        if (_terminalUI != null)
            _terminalUI.SetActive(true);

        _cctvController.Activate();
        _terminalCam.gameObject.SetActive(true);
        _monsterController.StartMoving();
    }

    public void CloseTerminal()
    {
        if (!_isOpened)
            return;

        _monsterController.StopMoving();
        _cctvController.Deactivate();

        if (_terminalUI != null)
            _terminalUI.SetActive(false);

        _inputRoot.SetMode(InputRoot.InputMode.Player);
        _playerRoot.TogglePlayerActivation(true);

        _isOpened = false;
        _terminalCam.gameObject.SetActive(false);
    }

    public void Shutdown()
    {
        UnsubscribeToEvents();
                   
        if (_isOpened)
            CloseTerminal();
    }

    private void OnPlayerCaught()
    {
        _monsterController.StopMoving();

        Debug.Log("GAME OVER: Monster caught the player!");

        CloseTerminal();

        // Позже подключим GameRoot и экран проигрыша.
    }

    #region >>> EVETNS

    private void SubscribeToEvents()
    {
        _inputRoot.TerminalExited += CloseTerminal;
        _inputRoot.NextCamera += _cctvController.NextCamera;
        _inputRoot.PreviousCamera += _cctvController.PreviousCamera;

        _monsterController.PlayerCaught += OnPlayerCaught;
    }

    private void UnsubscribeToEvents()
    {
        if (_inputRoot != null)
        {
            _inputRoot.TerminalExited -= CloseTerminal;
            _inputRoot.NextCamera -= _cctvController.NextCamera;
            _inputRoot.PreviousCamera -= _cctvController.PreviousCamera;
        }

        _monsterController.PlayerCaught -= OnPlayerCaught;
    }

    #endregion
}
