
using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFade : MonoBehaviour
{
    [SerializeField] private float _defaultDuration = 0.5f;

    private Tween _tween;
    private CanvasGroup _canvasGroup;
       
    public void Initialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        SetAlpha(1f);
    }

    // Показать чёрный экран
    public void FadeIn(float duration = -1f, Action onComplete = null)
    {
        FadeTo(1f, duration, onComplete);
    }

    // Убрать чёрный экран, показать игру
    public void FadeOut(float duration = -1f, Action onComplete = null)
    {
        FadeTo(0f, duration, onComplete);
    }

    private void FadeTo(
        float targetAlpha,
        float duration,
        Action onComplete)
    {
        _tween?.Kill();

        float time = duration < 0f
            ? _defaultDuration
            : duration;

        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = false;

        _tween = _canvasGroup
            .DOFade(targetAlpha, time)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                _canvasGroup.blocksRaycasts = targetAlpha > 0f;
                onComplete?.Invoke();
            });
    }

    public void SetAlpha(float alpha)
    {
        _tween?.Kill();

        _canvasGroup.alpha = Mathf.Clamp01(alpha);
        _canvasGroup.blocksRaycasts = alpha > 0f;
        _canvasGroup.interactable = false;
    }

    private void OnDestroy()
    {
        _tween?.Kill();
    }
}
