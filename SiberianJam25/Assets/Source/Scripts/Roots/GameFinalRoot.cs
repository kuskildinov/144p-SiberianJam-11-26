using IcePiecesGames.Framework.Core.Events;
using IcePiecesGames.Framework.Core.Lifecycle;
using IcePiecesGames.Framework.Audio;
using UnityEngine;
using DG.Tweening;

[DependsOn(typeof(PlayerRoot))]
[DependsOn(typeof(AudioRoot))]
[DependsOn(typeof(EventBusRoot))]
public class GameFinalRoot : FrameworkRoot, IComposable, IRunnable, IShutdownable
{
    [SerializeField] private GlassesPodium _glasses;
    [SerializeField] private FinalGameView _finalGameView;
    [Header("Fianl sound")]
    [SerializeField] private AudioSource _finalSource;
    [SerializeField] private AudioClip _happySound;
    [SerializeField] private float _duration = 5f;

    private RoomRoot _roomRoot;
    private IEventBus _eventBus;
    private IAudioService _audioService;

    private Sequence _transition;

    public void Compose()
    {
        _roomRoot = Context.GetRoot<RoomRoot>();
        _eventBus = Context.GetService<IEventBus>();
        _audioService = Context.GetService<IAudioService>();

        _glasses.Initialize(this);
        _finalGameView.Initialzie(this);
    }

    public void Run()
    {
        PrepareHappySound();
    }

    public void OnGlassesPutOn()
    {
        _finalGameView.Open();
        _eventBus.Publish(new GameFinalEvent());
    }
    
    public void BackToMainMenu()
    {
        _roomRoot.TryLoadNextLevel();
    }

    public void Shutdown()
    {
        _transition?.Kill();
        _finalGameView.Dispose();
    }

    #region >>> SOUND

    public void StartChangeSound()
    {
        _transition?.Kill();

        float startVolume = _audioService.MusicVolume;

        _finalSource.volume = 0f;
        _finalSource.Play();

        _transition = DOTween.Sequence();

        // Уменьшаем громкость музыки AudioService.
        _transition.Join(
            DOTween.To(
                () => _audioService.MusicVolume,
                value => _audioService.SetMusicVolume(value),
                0f,
                _duration
            ).SetEase(Ease.InOutSine)
        );

        // Одновременно увеличиваем громкость финальной музыки.
        _transition.Join(
            _finalSource
                .DOFade(0.5f, _duration)
                .SetEase(Ease.InOutSine)
        );

        _transition.OnComplete(() =>
        {
            _audioService.StopMusic();
        });
    }

    private void PrepareHappySound()
    {
        _finalSource.playOnAwake = false;
        _finalSource.loop = true;
        _finalSource.volume = 0f;
    }

   

    #endregion
}

public readonly struct GameFinalEvent
{

}
