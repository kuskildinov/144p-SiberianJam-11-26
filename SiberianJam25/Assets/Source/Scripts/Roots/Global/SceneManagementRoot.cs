using IcePiecesGames.Framework.Core.Lifecycle;
using IcePiecesGames.Framework.Core.SceneManagement;

namespace IcePiecesGames.Framework.Core.SceneManagement
{
    public sealed class SceneManagementRoot : FrameworkRoot, IComposable
    {
        public SceneTransitionService Service { get; private set; }

        public void Compose()
        {
            Service = new SceneTransitionService();

            Context.RegisterService<SceneTransitionService>( Service, owned: true);
        }
    }
}
