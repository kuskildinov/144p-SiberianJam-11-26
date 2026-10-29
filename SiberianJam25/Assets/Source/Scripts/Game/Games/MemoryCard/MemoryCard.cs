
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCardView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _back;
    [SerializeField] private Text _symbolText;

    private int _symbolId;
    private Action<MemoryCardView> _onClicked;

    public int SymbolId => _symbolId;
    public bool IsMatched { get; private set; }

    public void Initialize(int symbolId, string symbol,Action<MemoryCardView> onClicked)
    {
        _symbolId = symbolId;
        _onClicked = onClicked;
        IsMatched = false;

        _symbolText.text = symbol;
        Hide();
    }

    public void Reveal()
    {
        _back.SetActive(false);
        _symbolText.gameObject.SetActive(true);
        _button.interactable = false;
    }

    public void Hide()
    {
        if (IsMatched)
            return;

        _back.SetActive(true);
        _symbolText.gameObject.SetActive(false);
        _button.interactable = true;
    }

    public void SetMatched()
    {
        IsMatched = true;
        _button.interactable = false;
    }

    public void OnClick()
    {
        if (!IsMatched)
            _onClicked?.Invoke(this);
    }
}
