using System;
using UnityEngine;
using UnityEngine.UI;

public class TerminalStartView : MonoBehaviour,IDisposable
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _startButton; 

    private TerminalRoot _root;

    public void Initialize(TerminalRoot root)
    {
        _root = root;

        SubscribeToEvents();
    }

    #region >>> EVENTS

    public void Open()
    {
        _panel.gameObject.SetActive(true);
    }

    public void Close()
    {
        _panel.gameObject.SetActive(false);
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void UnsubscribeToEvents()
    {
        _startButton.onClick.RemoveListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        _root.TryStartMiniGame();
        _root.PlayTerminalButtonSound();
    }

    public void Dispose()
    {
        UnsubscribeToEvents();
    }

    #endregion


}
