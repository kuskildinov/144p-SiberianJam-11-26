using UnityEngine;
using UnityEngine.UI;

public class CCTVController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _cctvCamera;
    [SerializeField] private RawImage _cameraImage;
    [SerializeField] private RenderTexture _renderTexture;   
   
    public void Initialize()
    {
        if (_cctvCamera == null)
            return;
        
        _cctvCamera.targetTexture = _renderTexture;

        if (_cameraImage != null)
            _cameraImage.texture = _renderTexture;

        _cctvCamera.enabled = false;
    }

    public void Activate()
    {
        if (_cctvCamera != null)
            _cctvCamera.enabled = true;
    }

    public void Deactivate()    {
       
        if(_cctvCamera != null)
            _cctvCamera.enabled = false;
    }
}
