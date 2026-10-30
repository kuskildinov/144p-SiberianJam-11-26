using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCardView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _cardVisual;
    [SerializeField] private GameObject _back;
    [SerializeField] private GameObject _front;
    [SerializeField] private Image _cardImage;

    [Header("Animation")]
    [SerializeField] private float _flipDuration = 0.4f;
    [SerializeField] private Ease _flipEase = Ease.InOutQuad;

    private MemoryGame _root;
    private int _symbolId;
    private Tween _flipTween;

    private bool _isRevealed;
    private bool _isAnimating;

    public int SymbolId => _symbolId;
    public bool IsMatched { get; private set; }
    public bool IsRevealed => _isRevealed;

    public void Initialize(MemoryGame root, int symbolId,Sprite image)
    {
        _flipTween?.Kill();

        _root = root;
        _symbolId = symbolId;       

        IsMatched = false;
        _isRevealed = false;
        _isAnimating = false;
        _cardImage.sprite = image;

        _cardVisual.localRotation = Quaternion.identity;

        _back.SetActive(true);
        _front.SetActive(false);

        _button.interactable = true;

        SubscribeToEvents();
    }

    public void Reveal()
    {
        Flip(true);
    }

    public void Hide()
    {
        if (IsMatched)
            return;

        Flip(false);
    }

    private void Flip(bool reveal)
    {
        if (_isAnimating || _isRevealed == reveal)
            return;

        _flipTween?.Kill();

        _isAnimating = true;
        _button.interactable = false;

        float targetAngle = reveal ? 180f : 0f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_cardVisual.DOLocalRotate(new Vector3(0f, 90f, 0f),_flipDuration * 0.5f).SetEase(_flipEase));
        sequence.AppendCallback(() =>
        {
            _back.SetActive(!reveal);
            _front.SetActive(reveal);
        });
        sequence.Append(_cardVisual.DOLocalRotate(new Vector3(0f, targetAngle, 0f),_flipDuration * 0.5f).SetEase(_flipEase));
        sequence.OnComplete(() =>
        {
            _isRevealed = reveal;
            _isAnimating = false;

            _button.interactable = !IsMatched && !_isRevealed;
        });

        _flipTween = sequence;
    }

    public void CompleteFlip()
    {
        _flipTween?.Complete();
    }

    public void SetMatched()
    {
        IsMatched = true;
        _button.interactable = false;
    }    

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _button.onClick.AddListener(OnButtonClicked);
    
    }

    private void UnsubscribeToEvents()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void OnButtonClicked()
    {
        if (_isAnimating || _isRevealed || IsMatched)
            return;

        _root.OnCardClicked(this);
    }
    #endregion

    private void OnDestroy()
    {
        _flipTween?.Kill();

        UnsubscribeToEvents();
    }
}
