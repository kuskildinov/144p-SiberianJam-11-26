using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlotView : MonoBehaviour, IDropHandler
{
    [SerializeField] private int _slotId;

    private PuzzleGame _game;

    public void Initialize(PuzzleGame game)
    {
        _game = game;
    }

    public void OnDrop(PointerEventData eventData)
    {
        PuzzlePieceView piece = eventData.pointerDrag?.GetComponent<PuzzlePieceView>();

        if (piece == null)
            return;

        if (_game.TryPlacePiece(piece.PieceId, _slotId))
            piece.PlaceInto(transform);
    }
}