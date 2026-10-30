using UnityEngine;

public class ChangeableObject : MonoBehaviour, IInteractable
{
    [Header("Visuals")]
    [SerializeField] private GameObject _normalVisual;
    [SerializeField] private GameObject _changedVisual;

    private RoomChangeGame _game;

    private bool _isChanged;
    private bool _isFound;

    public string InteractionText =>
        _isChanged && !_isFound
            ? "E Ч ќтметить изменение"
            : string.Empty;

    public bool IsActive { get; set; } = true;

    public void Initialize(RoomChangeGame game)
    {
        _game = game;
        _isChanged = false;
        _isFound = false;

        UpdateVisual();
    }

    public void SetChanged(bool changed)
    {
        _isChanged = changed;
        UpdateVisual();
    }

    public void SetFound(bool found)
    {
        _isFound = found;
    }

    public void Interact(Player player)
    {
        if (_game == null)
            return;

        _game.TryFindChange(this);
    }

    private void UpdateVisual()
    {
        if (_normalVisual != null)
            _normalVisual.SetActive(!_isChanged);

        if (_changedVisual != null)
            _changedVisual.SetActive(_isChanged);
    }
}