using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovment _movment;
    [SerializeField] private PlayerInteraction _interaction;
    [Header("Other")]
    [SerializeField] private CinemachineCamera _playerCam;

    private PlayerRoot _root;
    private InputRoot _inputRoot;
    private bool _isActive;
    private CharacterController _controller;

    public bool IsActive => _isActive;
    public CinemachineCamera PlayerCam => _playerCam;
    public CharacterController Controller => _controller;

    public void Initialize(PlayerRoot root, InputRoot inputRoot)
    {
        _root = root;
        _inputRoot = inputRoot;
        _controller = GetComponent<CharacterController>();

        _movment.Initialize(this, _inputRoot);
        _interaction.Initialize(this, _inputRoot);
    }

    public void ToggleActivation(bool value)
    {
        _isActive = value;
    }

    #region >>> INTERACTION


    #endregion
}
