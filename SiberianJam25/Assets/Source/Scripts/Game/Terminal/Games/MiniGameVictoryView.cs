using UnityEngine;
using UnityEngine.UI;

public class MiniGameVictoryView : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _codeText;
    [SerializeField] private Button _continueButton;

    private TerminalRoot _root;

    public void Initialize(TerminalRoot root)
    {
        _root = root;

        Close();
        UpdateDoorCode();
        SubscribeToEvents();
    }

    #region >>> OPEN CLOSE

    public void Open()
    {      
        _panel.SetActive(true);
    }

    public void Close()
    {
        _panel.SetActive(false);
    }
    #endregion
    #region >>> DOOR CODE

    private void UpdateDoorCode()
    {
        _codeText.text = _root.DoorCode;
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void UnsubscribeToEvents()
    {
        _continueButton.onClick.RemoveAllListeners();
    }

    private void OnContinueButtonClicked()
    {
        _root.OnContinueClicked();
        _root.PlayTerminalButtonSound();
    }

    #endregion

    public void Dispose()
    {
        UnsubscribeToEvents();
    }
}
