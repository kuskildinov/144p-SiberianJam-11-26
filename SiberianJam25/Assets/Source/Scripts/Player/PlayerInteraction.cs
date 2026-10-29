using Unity.Cinemachine;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _interactionDistance = 2.5f;
    [SerializeField] private LayerMask _interactionMask = ~0;

    private Player _root;
    private CinemachineCamera _playerCam => _root.PlayerCam;

    private InputRoot _inputRoot;

    private IInteractable _currentInteractable;

    public void Initialize(Player root, InputRoot inputRoot)
    {
        _root = root;
        _inputRoot = inputRoot;

        SubscrivbeToEvents();
    }

    private void Update()
    {
        if (!_root.IsActive)
        {
            ClearInteraction();
            return;
        }

        FindInteractable();
    }

    private void FindInteractable()
    {
        _currentInteractable = null;

        Ray ray = new Ray( _playerCam.transform.position, _playerCam.transform.forward);
        if (Physics.Raycast( ray, out RaycastHit hit, _interactionDistance, _interactionMask, QueryTriggerInteraction.Ignore))
        {
           if(hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                _currentInteractable = interactable;
                Debug.Log(interactable);
            }
        }
    }

    public void OnInteract()
    {
        if (!_root.IsActive)
            return;

        _currentInteractable?.Interact(_root);
    }

    private void ClearInteraction()
    {
        _currentInteractable = null;
    }

    #region >>> EVENTS

    private void SubscrivbeToEvents()
    {
        _inputRoot.Interacted += OnInteract;
    }

    private void UnsubscribeToEvents()
    {
        _inputRoot.Interacted -= OnInteract;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
