using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySequenceGame : MiniGame
{
    [Header("Buttons")]
    [SerializeField] private SequenceButtonView[] _buttons;

    [Header("Settings")]
    [SerializeField, Min(1)] private int _roundsToWin = 5;
    [SerializeField] private float _flashDuration = 0.45f;
    [SerializeField] private float _pauseBetweenFlashes = 0.2f;
    [SerializeField] private float _roundDelay = 0.7f;

    private readonly List<int> _sequence = new();

    private Coroutine _routine;
    private int _inputIndex;
    private bool _started;
    private bool _acceptingInput;
    private bool _paused;
    private int _generation;

    public override void Initialize(MiniGamesService controller)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            int index = i;
            _buttons[i].Initialize(() => OnButtonPressed(index));
        }
    }

    public override void StartGame()
    {
        if (IsCompleted)
            return;

        if (!_started)
        {
            _started = true;
            _sequence.Clear();
            _inputIndex = 0;
        }

        _paused = false;
        StartRoutine(ShowNextRound());
    }

    public override void PauseGame()
    {
        _paused = true;
        _acceptingInput = false;

        StopCurrentRoutine();

        foreach (var button in _buttons)
            button.ResetVisual();
    }

    private IEnumerator ShowNextRound()
    {
        _acceptingInput = false;

        yield return new WaitForSeconds(_roundDelay);

        _sequence.Add(Random.Range(0, _buttons.Length));

        yield return ShowSequence();
    }

    private IEnumerator ShowSequence()
    {
        _acceptingInput = false;
        _inputIndex = 0;

        foreach (int index in _sequence)
        {
            _buttons[index].LightUp(_flashDuration);

            yield return new WaitForSeconds(
                _flashDuration + _pauseBetweenFlashes
            );
        }

        _acceptingInput = true;
        _routine = null;
    }

    private void OnButtonPressed(int index)
    {
        if (!_acceptingInput || _paused || IsCompleted)
            return;

        _buttons[index].LightUp(0.2f);

        if (_sequence[_inputIndex] != index)
        {
            _acceptingInput = false;
            StartRoutine(RetryRound());
            return;
        }

        _inputIndex++;

        if (_inputIndex < _sequence.Count)
            return;

        _acceptingInput = false;

        if (_sequence.Count >= _roundsToWin)
        {
            CompleteGame();
            return;
        }

        StartRoutine(ShowNextRound());
    }

    private IEnumerator RetryRound()
    {
        yield return new WaitForSeconds(0.8f);
        yield return ShowSequence();
    }

    private void StartRoutine(IEnumerator routine)
    {
        StopCurrentRoutine();
        _routine = StartCoroutine(routine);
    }

    private void StopCurrentRoutine()
    {
        if (_routine == null)
            return;

        StopCoroutine(_routine);
        _routine = null;
    }
}