using UnityEngine;
using UnityEngine.UI;

public class CCTVController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _cctvCamera;
    [SerializeField] private RawImage _cameraImage;
    [SerializeField] private RenderTexture _renderTexture;

    [Header("Camera Points")]
    [SerializeField] private Transform[] _cameraPoints;

    private int _currentCameraIndex;
    private bool _isActive;

    public int CurrentCameraIndex => _currentCameraIndex;

    public void Initialize()
    {
        _cctvCamera.targetTexture = _renderTexture;

        if (_cameraImage != null)
            _cameraImage.texture = _renderTexture;

        _cctvCamera.enabled = false;
    }

    public void Activate()
    {
        if (_cameraPoints == null || _cameraPoints.Length == 0)
            return;

        _isActive = true;
        SetCamera(0);
        _cctvCamera.enabled = true;
    }

    public void Deactivate()
    {
        _isActive = false;
        _cctvCamera.enabled = false;
    }

    public void NextCamera()
    {
        if (!_isActive)
            return;

        SetCamera(_currentCameraIndex + 1);
    }

    public void PreviousCamera()
    {
        if (!_isActive)
            return;

        SetCamera(_currentCameraIndex - 1);
    }

    private void SetCamera(int index)
    {
        if (_cameraPoints == null || _cameraPoints.Length == 0)
            return;

        _currentCameraIndex = (index % _cameraPoints.Length + _cameraPoints.Length) % _cameraPoints.Length;

        Transform point = _cameraPoints[_currentCameraIndex];
        _cctvCamera.transform.SetPositionAndRotation( point.position, point.rotation);
    }
}
