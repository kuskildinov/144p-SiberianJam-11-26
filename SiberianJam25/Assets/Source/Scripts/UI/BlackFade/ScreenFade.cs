using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFade : MonoBehaviour
{
    [SerializeField] private float _defaultDuration = 0.5f;

    private Tween tween;
    private CanvasGroup _canvasGroup;

    public void Initialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        SetAlpha(0f);
    }

    public void FadeIn(float duration = -1f, Action onComplete = null)
    {
        FadeTo(1f, duration, onComplete);
    }

    public void FadeOut(float duration = -1f, Action onComplete = null)
    {
        FadeTo(0f, duration, onComplete);
    }

    private void FadeTo(float alpha, float duration, Action onComplete)
    {
        tween?.Kill();

        float time = duration < 0f ? _defaultDuration : duration;

        _canvasGroup.blocksRaycasts = true;

        tween = _canvasGroup
            .DOFade(alpha, time)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                _canvasGroup.blocksRaycasts = alpha > 0f;
                onComplete?.Invoke();
            });
    }

    public void SetAlpha(float alpha)
    {
        tween?.Kill();

        _canvasGroup.alpha = Mathf.Clamp01(alpha);
        _canvasGroup.blocksRaycasts = alpha > 0f;
    }

    private void OnDestroy()
    {
        tween?.Kill();
    }
}
