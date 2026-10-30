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
    [SerializeField] private DoorSceneTrigger _doorTrigger;
    [SerializeField] private string _nextLevelSceneName;

    private IEventBus _eventBus;
    private SceneTransitionService _sceneTransitionService;

    public string DoorCode => _doorCode;

    public void Compose()
    {
        _eventBus = Context.GetService<IEventBus>();
        //if(Context.Parent.TryGetService<SceneTransitionService>(out SceneTransitionService sceneService))
        //    _sceneTransitionService = sceneService;

        _door.Initialize(this, _eventBus);
        _doorTrigger.Initialize(this);
    }

    public void OnDoorOpend()
    {

    }

    public async Task TryLoadNextLevel()
    {
        //if (_sceneTransitionService == null)
        //    return;

        //SceneTransitionResult result = await _sceneTransitionService.LoadSceneAsync(_nextLevelSceneName);

        //if (!result.Success)
        //{
        //    Debug.LogError( $"Failed to load scene: {result.Error}");
        //}

    }

    public void Shutdown()
    {
        _door.Dispose();
    }
}
