using UnityEngine;

public class ComputerTerminal : MonoBehaviour, IInteractable
{
    private TerminalRoot _root;

    public string InteractionText => "E Ч »спользовать терминал";

    public void Initialize(TerminalRoot root)
    {
        _root = root;
    }

    public void Interact(Player player)
    {
        _root.OpenTerminal();
    }
}
