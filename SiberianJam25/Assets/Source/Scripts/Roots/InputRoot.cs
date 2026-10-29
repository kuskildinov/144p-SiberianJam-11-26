using IcePiecesGames.Framework.Core.Lifecycle;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private InputAction _nextCameraAction;
    private InputAction _previousCameraAction;

    public InputMode CurrentMode { get; private set; }
    public Vector2 Move => CurrentMode == InputMode.Player? _moveAction.ReadValue<Vector2>() : Vector2.zero;
    public Vector2 Look => CurrentMode == InputMode.Player? _lookAction.ReadValue<Vector2>()  : Vector2.zero;

    public event Action Interacted;
    public event Action TerminalExited;
    public event Action NextCamera;
    public event Action PreviousCamera;

    public void Compose()
    {
        if(_inputActions == null)
            throw new InvalidOperationException("InputActionAsset is not assigned.");

        _inputActions = Instantiate(_inputActions);

        _playerMap = _inputActions.FindActionMap("Player", true);
        _terminalMap = _inputActions.FindActionMap("Terminal", true);

        _moveAction = _playerMap.FindAction("Move", true);
        _lookAction = _playerMap.FindAction("Look", true);
        _interactAction = _playerMap.FindAction("Interact", true);

        _exitAction = _terminalMap.FindAction("Exit", true);
        _nextCameraAction = _terminalMap.FindAction("NextCamera", true);
        _previousCameraAction = _terminalMap.FindAction("PreviousCamera", true);       
    }

    public void Run()
    {
        SubscribeToEvents();
        CurrentMode = InputMode.Disabled;

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

        bool cursorLocked = mode == InputMode.Player;

        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !cursorLocked;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Interacted?.Invoke();
    }

    private void OnExit(InputAction.CallbackContext context)
    {
        TerminalExited?.Invoke();
    }

    private void OnNextCamera(InputAction.CallbackContext context)
    {
        NextCamera?.Invoke();
    }

    private void OnPreviousCamera(InputAction.CallbackContext context)
    {
        PreviousCamera?.Invoke();
    }

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _interactAction.performed += OnInteract;
        _exitAction.performed += OnExit;
        _nextCameraAction.performed += OnNextCamera;
        _previousCameraAction.performed += OnPreviousCamera;
    }

    private void UnsubscribeToEvents()
    {
        _interactAction.performed -= OnInteract;
        _exitAction.performed -= OnExit;
        _nextCameraAction.performed -= OnNextCamera;
        _previousCameraAction.performed -= OnPreviousCamera;
    }
    #endregion

    public void Shutdown()
    {
        _playerMap?.Disable();
        _terminalMap?.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UnsubscribeToEvents();
        Destroy(_inputActions);
    }
}
