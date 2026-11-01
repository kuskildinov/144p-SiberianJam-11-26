using UnityEngine;
using UnityEngine.UI;

public class RoomChangeGameView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RoomChangeGame _game;
    [SerializeField] private Button _startButton;
    [SerializeField] private Text _statusText;
    [SerializeField] private Text _progressText;

    private void Start()
    {
        _startButton.onClick.AddListener(OnStartClicked);

        _game.ProcedureStartedEvent += OnProcedureStarted;
        _game.ProgressChanged += OnProgressChanged;

        Refresh();
    }

    private void OnStartClicked()
    {
        _game.StartProcedure();
    }

    private void OnProcedureStarted()
    {
        Refresh();
    }

    private void OnProgressChanged(int found, int total)
    {
        _progressText.text = $"{found}/{total}";
    }

    private void Refresh()
    {
        bool started = _game.ProcedureStarted;
        _startButton.gameObject.SetActive(!started);
        _statusText.text = started ? "Найдите изменения в помещении" : "Осмотрите помещение перед проверкой";
        _progressText.text = $"{_game.FoundCount}/{_game.ChangesCount}";
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(OnStartClicked);

        if (_game == null)
            return;

        _game.ProcedureStartedEvent -= OnProcedureStarted;
        _game.ProgressChanged -= OnProgressChanged;
    }
}