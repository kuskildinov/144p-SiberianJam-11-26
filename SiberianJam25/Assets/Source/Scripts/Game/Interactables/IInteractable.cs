
public interface IInteractable
{
    string InteractionText { get; }
    bool IsActive { get; set; }

    void Interact(Player player);
}
