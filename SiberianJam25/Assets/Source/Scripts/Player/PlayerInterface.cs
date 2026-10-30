using IcePiecesGames.Framework.Core.Events;
using System;
using UnityEngine;

public class PlayerInterface : MonoBehaviour, IDisposable
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private InteractionInfoPanel _interactionInfo;

    private PlayerRoot _root;

    private IEventBus _eventBus;
    private IDisposable _interactorDetectedSubscription;
    private IDisposable _interactorLostSubscription;

    public void Initialize(PlayerRoot root, IEventBus eventBus)
    {
        _root = root;
        _eventBus = eventBus;

        ToggleInteractionInfo(false);
        SubscribeToEvents();
    }

    public void ToggleVisibility(bool value)
    {
        _panel.gameObject.SetActive(value);

        ToggleInteractionInfo(false);
    }

    #region >>>INTERACTION INFO

    public void ToggleInteractionInfo(bool value, string info = "")
    {
        _interactionInfo.ToggleVisibility(value,info);
    }

    #endregion
    #region >>> EVETNS

    private void SubscribeToEvents()
    {
        if(_interactorDetectedSubscription == null)
            _interactorDetectedSubscription = _eventBus.Subscribe<InteractiorDetectedEvent>(OnInteractorDetected);

        if (_interactorLostSubscription == null)
            _interactorLostSubscription = _eventBus.Subscribe<InteractorLostEvent>(OnInteractorLost);
    }

    private void UnsubscribeFromEvents()
    {
        _interactorDetectedSubscription?.Dispose();
        _interactorDetectedSubscription = null;

        _interactorLostSubscription?.Dispose();
        _interactorLostSubscription = null;
    }

    private void OnInteractorDetected(InteractiorDetectedEvent eventData)
    {        
        ToggleInteractionInfo(true, eventData.InteractorInfo);
    }

    private void OnInteractorLost(InteractorLostEvent eventData)
    {
        Debug.Log("“ÛÚ");
        ToggleInteractionInfo(false);
    }

    #endregion

    public void Dispose()
    {
        UnsubscribeFromEvents();
    }
}
