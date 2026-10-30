using IcePiecesGames.Framework.Core.Events;
using IcePiecesGames.Framework.Core.Lifecycle;
using IcePiecesGames.Framework.Core.SceneManagement;
using System.Threading.Tasks;
using UnityEngine;

[DependsOn(typeof(EventBusRoot))]
public class RoomRoot : FrameworkRoot, IComposable,IShutdownable
{
    [SerializeField] private RoomDoor _door;
    [SerializeField] private string _doorCode = "4826";
    [Header("Next Level")]
    [SerializeField] private string _currentSceneName;
    [SerializeField] private string _nextLevelSceneName;

    private IEventBus _eventBus;
    private SceneTransitionService _sceneTransitionService;

    public string DoorCode => _doorCode;

    public void Compose()
    {
        _eventBus = Context.GetService<IEventBus>();
        if(Context.TryGetService<SceneTransitionService>(out SceneTransitionService sceneService))
            _sceneTransitionService = sceneService;

        _door.Initialize(this, _eventBus);
    }

    public async Task TryLoadNextLevel()
    {
        await LoadScene(_nextLevelSceneName);
    }

    public async Task RestartLevel()
    {
        await LoadScene(_currentSceneName);
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
        _door.Dispose();
    }
}
