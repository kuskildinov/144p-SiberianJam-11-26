using IcePiecesGames.Framework.Audio;
using IcePiecesGames.Framework.Core.Events;
using IcePiecesGames.Framework.Core.Lifecycle;
using IcePiecesGames.Framework.Core.SceneManagement;
using System.Threading.Tasks;
using UnityEngine;

[DependsOn(typeof(EventBusRoot))]
[DependsOn(typeof(AudioRoot))]
public class RoomRoot : FrameworkRoot, IComposable,IRunnable, IShutdownable
{
    [SerializeField] private RoomDoor _door;
    [SerializeField] private string _doorCode = "4826";
    [Header("Next Level")]
    [SerializeField] private string _currentSceneName;
    [SerializeField] private string _nextLevelSceneName;   
    [Header("Sounds")]
    [SerializeField] private AudioClip _backgroundSound;

    private IEventBus _eventBus;
    private IAudioService _audioService;
    private SceneTransitionService _sceneTransitionService;

    public string DoorCode => _doorCode;

    public void Compose()
    {
        _eventBus = Context.GetService<IEventBus>();
        if(Context.TryGetService<SceneTransitionService>(out SceneTransitionService sceneService))
            _sceneTransitionService = sceneService;

        if (Context.TryGetService<IAudioService>(out IAudioService audioService))
            _audioService = audioService;

        if (_door != null)
            _door.Initialize(this, _eventBus);
    }

    public void Run()
    {
        if(_audioService != null)
        {
            _audioService.SetMusicVolume(0.5f);
            _audioService.PlayMusic(_backgroundSound, loop: true);
        }
          
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
        if(_door != null)
            _door.Dispose();
    }

    
}
