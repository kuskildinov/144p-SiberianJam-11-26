using UnityEngine;

public class ComputerTerminal : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionText;
    private TerminalRoot _root;

    public string InteractionText => _interactionText;

    public void Initialize(TerminalRoot root)
    {
        _root = root;
    }

    public void Interact(Player player)
    {
        _root.OpenTerminal();
    }
}
