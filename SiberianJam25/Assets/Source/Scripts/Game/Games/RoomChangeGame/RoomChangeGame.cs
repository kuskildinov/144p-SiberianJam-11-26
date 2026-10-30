
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomChangeGame : MiniGame
{
    [Header("Objects")]
    [SerializeField] private ChangeableObject[] _objects;

    [Header("Settings")]
    [SerializeField, Min(1)] private int _changesToFind = 3;

    private readonly HashSet<ChangeableObject> _changedObjects = new();
    private readonly HashSet<ChangeableObject> _foundObjects = new();

    private bool _procedureStarted;

    public int ChangesCount => _changedObjects.Count;
    public int FoundCount => _foundObjects.Count;
    public bool ProcedureStarted => _procedureStarted;

    public event Action<int, int> ProgressChanged;
    public event Action ProcedureStartedEvent;

    public override void Initialize(MiniGamesService controller)
    {
        foreach (ChangeableObject obj in _objects)
        {
            if (obj == null)
                continue;

            obj.Initialize(this);
            obj.SetChanged(false);
        }
    }

    public override void StartGame()
    {
        // Открытие терминала не запускает изменения.
        // Игрок должен нажать кнопку "Начать проверку".
    }

    public override void PauseGame()
    {
        // Изменения и прогресс сохраняются
        // при выходе из терминала.
    }

    public void StartProcedure()
    {
        if (_procedureStarted || IsCompleted)
            return;

        _procedureStarted = true;

        List<ChangeableObject> candidates = new();

        foreach (ChangeableObject obj in _objects)
        {
            if (obj != null && !candidates.Contains(obj))
                candidates.Add(obj);
        }

        int count = Mathf.Min(_changesToFind, candidates.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(
                0, candidates.Count
            );

            ChangeableObject selected = candidates[randomIndex];
            candidates.RemoveAt(randomIndex);

            _changedObjects.Add(selected);
            selected.SetChanged(true);
        }

        ProcedureStartedEvent?.Invoke();
        ProgressChanged?.Invoke(0, ChangesCount);
    }

    public void TryFindChange(ChangeableObject obj)
    {
        if (!_procedureStarted || IsCompleted)
            return;

        if (!_changedObjects.Contains(obj))
            return;

        if (!_foundObjects.Add(obj))
            return;

        obj.SetFound(true);

        ProgressChanged?.Invoke(
            _foundObjects.Count,
            _changedObjects.Count
        );

        if (_foundObjects.Count >= _changedObjects.Count)
            CompleteGame();
    }
}
