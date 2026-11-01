using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System;

public class FinalGameView : MonoBehaviour,IDisposable
{   
    [SerializeField] private GameObject _panel;
    [SerializeField] private CanvasGroup _happyPanel;
    [SerializeField] private ScreenFade _fade;
    [SerializeField] private Button _endButton;

    private GameFinalRoot _root;
       
    private Tween _happyFadeTween;

    public void Initialzie(GameFinalRoot root)
    {
        _root = root;

        SubscribeToEvents();
    }

    public void Open()
    {              
        StartCoroutine(FinalRoutine());
    }

    private IEnumerator FinalRoutine()
    {
        _fade.FadeIn(2f);

         yield return new WaitForSeconds(3f);
        _panel.gameObject.SetActive(true);
        _fade.FadeOut(2f);

        yield return new WaitForSeconds(3f);

        _root.StartChangeSound();
        _happyFadeTween = _happyPanel
            .DOFade(1f, 5f)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                _endButton.gameObject.SetActive(true);
            });
    }

    #region >>> EVETNS

    private void SubscribeToEvents()
    {
        _endButton.onClick.AddListener(OnEndButtonClicked);
    }

    private void UnsubscribeToEvetns()
    {
        _endButton.onClick.RemoveAllListeners();
    }

    private void OnEndButtonClicked()
    {
        _root.BackToMainMenu();
    }

    #endregion

    public void Dispose()
    {
        UnsubscribeToEvetns();
    }
}
