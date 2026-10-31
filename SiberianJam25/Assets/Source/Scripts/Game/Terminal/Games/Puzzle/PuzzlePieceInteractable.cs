using UnityEngine;

public class PuzzlePieceInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private int _pieceId;
    private PuzzleGame _root;

    public string InteractionText => "E — Подобрать фрагмент";

    public bool IsActive { get; set; } = true;
    public int PieceId => _pieceId;

    public void Initialize(PuzzleGame root)
    {
        _root = root;

        ToggleVisual(false);
    }

    public void Interact(Player player)
    {
        _root.CollectPiece(this);        
    }

    public void ToggleVisual(bool value)
    {
        this.gameObject.SetActive(value);
        IsActive = value;
    }
}