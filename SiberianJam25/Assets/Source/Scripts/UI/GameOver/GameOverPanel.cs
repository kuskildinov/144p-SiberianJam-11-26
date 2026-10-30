using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverPanel : MonoBehaviour, IDisposable
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Camera _monsterCam;

    private RoomRoot _roomRoot;
    private bool _isOpen;

    public void Initialize(RoomRoot roomRoot)
    {
        _roomRoot = roomRoot;
        _restartButton.interactable = true;
        _restartButton.gameObject.SetActive(false);
        SubscribeToEvents();
    }
    #region  >>> OPEN CLOSE
    public void Open()
    {
        if (_isOpen)
            return;

        _panel.gameObject.SetActive(true);
        _isOpen = true;
        StartCoroutine(OpenPanelRoutine());
    }

    public void Close()
    {
        _isOpen = false;
        _panel.gameObject.SetActive(false);
    }

    public void ChangeFOV(float targetFOV, float duration, Action OnComplete = null)
    {
        _monsterCam.DOFieldOfView(targetFOV, duration)
            .SetEase(Ease.InOutSine).OnComplete(() =>
            {
                OnComplete?.Invoke();
            });
    }

    private IEnumerator OpenPanelRoutine()
    {
        ChangeFOV(140, 3, () =>
        {
            ChangeFOV(90, 5);
        });
        yield return new WaitForSecondsRealtime(1f);
        _restartButton.gameObject.SetActive(true);
        yield return null;
    }

    #endregion

    private void RestartLevel()
    {
        _restartButton.interactable = false;
        _roomRoot.RestartLevel();
    }
    
    #region >> EVENTS

    private void SubscribeToEvents()
    {
        _restartButton.onClick.AddListener(OnGameOverButtonClicked);
    }

    private void UnsubscribeToEvents()
    {
        _restartButton.onClick.RemoveListener(OnGameOverButtonClicked);
    }

    private void OnGameOverButtonClicked()
    {
        RestartLevel();
    }

    #endregion

    public void Dispose()
    {
        UnsubscribeToEvents();
    }
}
