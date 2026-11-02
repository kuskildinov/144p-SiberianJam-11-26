using IcePiecesGames.Framework.Core.Lifecycle;
using IcePiecesGames.Framework.Core.SceneManagement;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRoot : FrameworkRoot, IComposable, IShutdownable
{
    [SerializeField] private string _nextLevelSceneName;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;

    private SceneTransitionService _sceneTransitionService;

    public void Compose()
    {
        if (Context.TryGetService<SceneTransitionService>(out SceneTransitionService sceneService))
            _sceneTransitionService = sceneService;

        _startButton.onClick.AddListener(() =>
        {
            TryLoadNextLevel();
        });

        _exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public async Task TryLoadNextLevel()
    {
        await LoadScene(_nextLevelSceneName);
    }

    public async Task LoadScene(string name)
    {
        if (_sceneTransitionService == null)
            return;

        SceneTransitionResult result = await _sceneTransitionService.LoadSceneAsync(name);

        if (!result.Success)
        {
            Debug.LogError($"Failed to load scene: {result.Error}");
        }
    }

    public void Shutdown()
    {
        _startButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
}
