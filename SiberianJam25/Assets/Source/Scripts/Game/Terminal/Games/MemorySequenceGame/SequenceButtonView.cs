using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SequenceButtonView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _normalColor = Color.gray;
    [SerializeField] private Color _activeColor = Color.yellow;

    private Button _button;
    private Tween _tween;
    private Action _onPressed;

    public void Initialize(Action onPressed)
    {
        _button = GetComponent<Button>();
        _onPressed = onPressed;

        _button.onClick.RemoveListener(HandleClick);
        _button.onClick.AddListener(HandleClick);

        ResetVisual();
    }

    public void LightUp(float duration)
    {
        _tween?.Kill();

        _image.color = _activeColor;

        _tween = _image.DOColor(
            _normalColor,
            duration
        ).SetEase(Ease.InQuad);
    }

    public void ResetVisual()
    {
        _tween?.Kill();
        _image.color = _normalColor;
    }

    private void HandleClick()
    {
        _onPressed?.Invoke();
    }

    private void OnDestroy()
    {
        _tween?.Kill();

        if (_button != null)
            _button.onClick.RemoveListener(HandleClick);
    }
}
