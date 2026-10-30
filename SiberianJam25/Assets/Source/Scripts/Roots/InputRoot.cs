using IcePiecesGames.Framework.Core.Events;
using IcePiecesGames.Framework.Core.Lifecycle;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DependsOn(typeof(EventBusRoot))]
public class InputRoot : FrameworkRoot,IComposable,IRunnable, IShutdownable
{
    public enum InputMode{ Player,Terminal, Disabled }

    [Header("Input")]
    [SerializeField] private InputActionAsset _inputActions;

    private InputActionMap _playerMap;
    private InputActionMap _terminalMap;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _interactAction;
    private InputAction _sprintAction;

    private InputAction _exitAction;   

    public InputMode CurrentMode { get; private set; }
    public Vector2 Move => CurrentMode == InputMode.Player? _moveAction.ReadValue<Vector2>() : Vector2.zero;
    public Vector2 Look => CurrentMode == InputMode.Player? _lookAction.ReadValue<Vector2>()  : Vector2.zero;

    public event Action Interacted;
    public event Action TerminalExited;
    public event Action NextCamera;
    public event Action PreviousCamera;

    private IEventBus _eventBus;
    private IDisposable _openTerminalSubscription;
    private IDisposable _closeTerminalSubscription;

    public void Compose()
    {       
        _eventBus = Context.GetService<IEventBus>();

        _inputActions = Instantiate(_inputActions);

        _playerMap = _inputActions.FindActionMap("Player", true);
        _terminalMap = _inputActions.FindActionMap("Terminal", true);

        _moveAction = _playerMap.FindAction("Move", true);
        _lookAction = _playerMap.FindAction("Look", true);
        _interactAction = _playerMap.FindAction("Interact", true);

        _exitAction = _terminalMap.FindAction("Exit", true);        
    }

    public void Run()
    {
        SubscribeToEvents();

        CurrentMode = InputMode.Disabled;
        ToggleCursor(false);
        if (_playerMap != null)
            SetMode(InputMode.Player);
    }

    public void SetMode(InputMode mode)
    {
        _playerMap.Disable();
        _terminalMap.Disable();

        CurrentMode = mode;

        switch (mode)
        {
            case InputMode.Player:
                _playerMap.Enable();
                break;

            case InputMode.Terminal:
                _terminalMap.Enable();
                break;
        }
       
        ToggleCursor(mode == InputMode.Player ? false : true);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Interacted?.Invoke();
    }

    private void OnExit(InputAction.CallbackContext context)
    {
        TerminalExited?.Invoke();
    }
       
    #region >>> MOUSE CURSOR

    private void ToggleCursor(bool value)
    {       
        Cursor.lockState = value? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _interactAction.performed += OnInteract;
        _exitAction.performed += OnExit;       

        if(_openTerminalSubscription == null)
            _openTerminalSubscription = _eventBus.Subscribe<TerminalOpenEvent>(OnTerminalOpend);
        if (_closeTerminalSubscription == null) 
            _closeTerminalSubscription = _eventBus.Subscribe<TerminalCloseEvent>(OnTerminalClosed);
    }

    private void UnsubscribeToEvents()
    {
        _interactAction.performed -= OnInteract;
        _exitAction.performed -= OnExit;
       
        _openTerminalSubscription?.Dispose();
        _openTerminalSubscription = null;

        _closeTerminalSubscription?.Dispose();
        _closeTerminalSubscription = null;
    }

    private void OnTerminalOpend(TerminalOpenEvent eventData)
    {
        SetMode(InputRoot.InputMode.Terminal);       
    }

    private void OnTerminalClosed(TerminalCloseEvent eventData)
    {
        SetMode(InputRoot.InputMode.Player);        
    }
    #endregion

    public void Shutdown()
    {
        _playerMap?.Disable();
        _terminalMap?.Disable();

        ToggleCursor(true);
        Destroy(_inputActions);
        UnsubscribeToEvents();
    }
}
