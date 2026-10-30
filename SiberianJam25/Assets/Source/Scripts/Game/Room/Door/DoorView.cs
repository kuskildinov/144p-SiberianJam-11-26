using UnityEngine;

public class DoorView : MonoBehaviour, IInteractable
{
    [SerializeField] private string _intereactionText;

    public string InteractionText => _intereactionText;

    private RoomDoor _root;
    private bool _isActive;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public void Initilize(RoomDoor root)
    {
        _root = root;
        _isActive = false;
    }

    public void Interact(Player player)
    {        
        _root.TryOpenDoor();
    }
}
