using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoorLock : MonoBehaviour
{
    private const int MaxCodeLenght = 4;

    public enum DoorLockState { Blocked, Active, Open}

    [SerializeField] private Text _currentCodeText;
    [SerializeField] private LockButton[] _buttons;

    private RoomDoor _door;

    private string _currentEnteredCode;
    private DoorLockState _currentState;

    public void Initialize(RoomDoor door)
    {
        _door = door;
        InitializeButtons();
        SetLockState(DoorLockState.Blocked);
    }

    public void OnButtonClicked(string num)
    {
        if (_currentState != DoorLockState.Active)
            return;
        
        TryAddCode(num);
    }

    private void InitializeButtons()
    {
        foreach (LockButton button in _buttons)
        {
            button.Initialize(this);
        }
    }

    private void ToggleButtonsActivation(bool value)
    {
        foreach (LockButton button in _buttons)
        {
            button.ToggleActivation(value);
        }
    }

    private void TryAddCode(string num)
    {
        if(_currentCodeText.text.Length >= MaxCodeLenght)
        {           
            ClearCode();
        }
        else
        {
            _currentEnteredCode += num;
            UpdateCodeText();
            CheckCode();
        }       
    }

    private void CheckCode()
    {
        if(_currentEnteredCode.Length == MaxCodeLenght)
            StartCoroutine(CheckCodeRoutine());
    }

    private void OnCorrectCodeEntered()
    {
        SetLockState(DoorLockState.Open);
        _door.OnCorrectCodeEntered();
    }

    private void OnIncorrectCodeEntered()
    {
        _currentCodeText.color = Color.red;
        _currentCodeText.text = "Не верный код";
    }    

    private void UpdateCodeText()
    {
        _currentCodeText.text = _currentEnteredCode;
    }

    private void ClearCode()
    {
        _currentEnteredCode = "";
        _currentCodeText.color = Color.green;
        UpdateCodeText();
    }

    public void SetLockState(DoorLockState newState)
    {
        _currentState = newState;

        switch (_currentState)
        {
            case DoorLockState.Blocked:
                _currentCodeText.color = Color.yellow;
                _currentCodeText.text = "Заблокировано";
                ToggleButtonsActivation(false);
                break;
            case DoorLockState.Active:
                _currentCodeText.color = Color.green;
                _currentCodeText.text = "";
                ToggleButtonsActivation(true);
                break;
            case DoorLockState.Open:
                _currentCodeText.color = Color.green;
                _currentCodeText.text = "Проход открыт";
                ToggleButtonsActivation(false);
                break;
            default:
                break;
        }
    }

    private IEnumerator CheckCodeRoutine()
    {
        ToggleButtonsActivation(false);
        if (_currentEnteredCode == _door.DoorCode)
        {
            yield return new WaitForSecondsRealtime(1f);
            OnCorrectCodeEntered();
        }
        else
        {
            yield return new WaitForSecondsRealtime(1f);
            OnIncorrectCodeEntered();
            yield return new WaitForSecondsRealtime(3f);
            ClearCode();
            ToggleButtonsActivation(true);
        }
    }
}