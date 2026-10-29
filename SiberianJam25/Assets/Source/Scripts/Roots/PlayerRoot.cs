using IcePiecesGames.Framework.Core.Lifecycle;
using UnityEngine;

[DependsOn(typeof(InputRoot))]
public class PlayerRoot : FrameworkRoot, IComposable,IRunnable
{
    [SerializeField] private Player _player;

    private InputRoot _inputRoot;

    public void Compose()
    {
        _inputRoot = Context.GetRoot<InputRoot>();

        _player.Initialize(this, _inputRoot);
    }

    public void Run()
    {
        _player.ToggleActivation(true);
    }

    public void TogglePlayerActivation(bool value)
    {
        _player.ToggleActivation(value);
    }
}
