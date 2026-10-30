using UnityEngine;
using UnityEngine.UI;

public class InteractionInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _infoText;
        
    public void ToggleVisibility(bool value, string info = "")
    {
        _panel.gameObject.SetActive(value);
        _infoText.text = info;
    }
}
