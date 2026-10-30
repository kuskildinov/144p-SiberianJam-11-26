using UnityEngine;

public class LockButton : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionText;
    [SerializeField] private string _num;

    private DoorLock _lock;
    private bool _isActive;
   
    public string InteractionText => _interactionText;
    public bool IsActive { get => _isActive; set => _isActive = value; }

    public void Initialize(DoorLock doorLock)
    {
        _lock = doorLock;
    }

    public void Interact(Player player)
    {
        if (!IsActive)
            return;

        _lock.OnButtonClicked(_num);
    }

    public void ToggleActivation(bool value)
    {
        _isActive = value;
    }
}
