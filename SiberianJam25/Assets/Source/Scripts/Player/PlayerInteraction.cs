using System;
using Unity.Cinemachine;
using UnityEngine;
using IcePiecesGames.Framework.Core.Events;

public class PlayerInteraction : MonoBehaviour, IDisposable
{
    [Header("Settings")]
    [SerializeField] private float _interactionDistance = 2.5f;
    [SerializeField] private LayerMask _interactionMask = ~0;

    private Player _root;
    private CinemachineCamera _playerCam => _root.PlayerCam;

    private InputRoot _inputRoot;

    private IEventBus _eventsBus;
    private IInteractable _currentInteractable;

    private bool _isDisposed;

    public void Initialize(Player root, InputRoot inputRoot,IEventBus eventBus)
    {
        _root = root;
        _inputRoot = inputRoot;
        _eventsBus = eventBus;

        SubscrivbeToEvents();
    }

    private void Update()
    {
        if (_isDisposed)
            return;

        if (!_root.IsActive)
        {
            ClearInteraction();
            return;
        }

        FindInteractable();
    }

    private void FindInteractable()
    {
        IInteractable detectedInteractable = null;

        Ray ray = new Ray( _playerCam.transform.position, _playerCam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, _interactionMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                if (!interactable.IsActive)
                    return;

                detectedInteractable = interactable;
            }
        }

        SetCurrentInteractable(detectedInteractable);
    }

    public void OnInteract()
    {
        if (!_root.IsActive)
            return;

        _currentInteractable?.Interact(_root);
    }

    private void SetCurrentInteractable(IInteractable interactable)
    {
        // Объект не изменился
        if (ReferenceEquals(_currentInteractable, interactable))
            return;

        // Потеряли предыдущий объект
        if (_currentInteractable != null)
        {
            OnInteractorLost();
        }

        _currentInteractable = interactable;

        // Обнаружили новый объект
        if (_currentInteractable != null)
        {
            OnInteractorDetected(_currentInteractable);
        }
    }

    private void OnInteractorDetected(IInteractable interactable)
    {        
        _eventsBus.Publish(new InteractiorDetectedEvent(_currentInteractable.InteractionText));
    }

    private void OnInteractorLost()
    {      
        _eventsBus.Publish(new InteractorLostEvent());
    }

    private void ClearInteraction()
    {
        SetCurrentInteractable(null);
    }

    #region >>> EVENTS

    private void SubscrivbeToEvents()
    {
        _inputRoot.Interacted += OnInteract;
    }

    private void UnsubscribeToEvents()
    {
        _inputRoot.Interacted -= OnInteract;
    }

    #endregion
   
    public void Dispose()
    {
        if (_isDisposed)
            return;
        _isDisposed = true;
        enabled = false;

        ClearInteraction();
        UnsubscribeToEvents();
    }
}
