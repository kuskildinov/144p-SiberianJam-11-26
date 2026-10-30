using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class PuzzlePieceView : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private int _pieceId;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Canvas _canvas;
    private Transform _originalParent;
    private Vector2 _originalPosition;

    private bool _collected;
    private bool _placed;
    private RectTransform _dragPlane;
    private bool _isDragging;
    private Vector2 _dragOffset;

    public int PieceId => _pieceId;

    public void Initialize(PuzzleGame game)
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();

        _originalParent = transform.parent;
        _originalPosition = _rectTransform.anchoredPosition;
    }

    public void SetCollected(bool collected)
    {
        _collected = collected;
        gameObject.SetActive(collected);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_collected || _placed)
            return;

        _isDragging = true;
        _canvasGroup.blocksRaycasts = false;

        _dragPlane = _rectTransform.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _dragPlane,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            _dragOffset = _rectTransform.anchoredPosition - localPoint;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging || _dragPlane == null)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _dragPlane,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            _rectTransform.anchoredPosition = localPoint + _dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;

        _isDragging = false;
        _canvasGroup.blocksRaycasts = true;

        if (!_placed)
            _rectTransform.anchoredPosition = _originalPosition;
    }

    public void PlaceInto(Transform slot)
    {
        _placed = true;

        transform.SetParent(slot, false);

        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition = Vector2.zero;

        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = false;
    }

    private void OnDisable()
    {
        if (_canvasGroup != null)
            _canvasGroup.blocksRaycasts = true;
    }
}
