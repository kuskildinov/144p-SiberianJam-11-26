using System;
using UnityEngine;
using UnityEngine.UI;

public class TerminalCameraView : MonoBehaviour,IDisposable
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _backButton;

    private TerminalRoot _root;

    public void Initialize(TerminalRoot root)
    {
        SubscribeToEvetns();
    }

    #region >>> OPEN CLOSE
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

    private void SubscribeToEvetns()
    {
        _openButton.onClick.AddListener(OnOpenButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void UnsubscribeToEvents()
    {
        _openButton.onClick.RemoveListener(OnOpenButtonClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
    }

    private void OnOpenButtonClicked()
    {
        Open();
        _root.PlayTerminalButtonSound();
    }

    private void OnBackButtonClicked()
    {
        Close();
        _root.PlayTerminalButtonSound();
    }

    #endregion

    public void Dispose()
    {
        UnsubscribeToEvents();
    }    
}
