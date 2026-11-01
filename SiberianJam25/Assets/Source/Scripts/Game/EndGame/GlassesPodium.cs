using UnityEngine;

public class GlassesPodium : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionText;

    public string InteractionText => _interactionText;

    public bool IsActive { get; set; } = true;

    private GameFinalRoot _root;

    public void Initialize(GameFinalRoot root)
    {
        _root = root;
    }

    public void Interact(Player player)
    {
        _root.OnGlassesPutOn();
    }
}
