using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _backMenuButton;

    private RoomRoot _roomRoot;

    public void Initialize(RoomRoot roomRoot)
    {
        _roomRoot = roomRoot;

        _backMenuButton.onClick.AddListener(() =>
        {
            _roomRoot.BackToMainMenu();
        });

        Close();
    }

    public void Open()
    {
        _panel.gameObject.SetActive(true);
    }

    public void Close()
    {
        _panel.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _backMenuButton.onClick.RemoveAllListeners();
    }

}
