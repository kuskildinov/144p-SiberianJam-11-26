using System.Collections.Generic;
using UnityEngine;

public class PuzzleGame : MiniGame
{
    [Header("Puzzle")]
    [SerializeField] private PuzzlePieceView[] _pieces;
    [SerializeField] private PuzzleSlotView[] _slots;
    [SerializeField] private PuzzlePieceInteractable[] _interactables;

    private readonly HashSet<int> _collectedPieces = new();
    private readonly HashSet<int> _placedPieces = new();

    private bool _started;

    public override void Initialize(MiniGamesService controller)
    {
        IsCompleted = false;

        foreach (PuzzlePieceInteractable interactable in _interactables)
            interactable.Initialize(this);

        foreach (PuzzlePieceView piece in _pieces)
        {
            piece.Initialize(this);
            piece.SetCollected(_collectedPieces.Contains(piece.PieceId));
        }

        foreach (PuzzleSlotView slot in _slots)
            slot.Initialize(this);
    }

    public override void StartGame()
    {
        if (_started)
            return;

        _started = true;
        TogglePiecesVisible(true);
    }

    public override void PauseGame()
    {
       
    }

    public void CollectPiece(PuzzlePieceInteractable interactablePiece)
    {
        if (IsCompleted)
            return;

        int pieceId = interactablePiece.PieceId;

        if (!_collectedPieces.Add(pieceId))
            return;

        interactablePiece.ToggleVisual(false);

        foreach (PuzzlePieceView piece in _pieces)
        {
            if (piece.PieceId != pieceId)
                continue;

            piece.SetCollected(true);
            break;
        }
    }

    public bool TryPlacePiece(int pieceId, int slotId)
    {
        if (!_collectedPieces.Contains(pieceId))
            return false;

        if (_placedPieces.Contains(pieceId))
            return false;

        if (pieceId != slotId)
            return false;

        _placedPieces.Add(pieceId);

        if (_placedPieces.Count == _pieces.Length)
            CompleteGame();

        return true;
    }

    public void TogglePiecesVisible(bool value)
    {
        foreach (PuzzlePieceInteractable interactable in _interactables)
        {
            interactable.ToggleVisual(value);
        }
    }
}