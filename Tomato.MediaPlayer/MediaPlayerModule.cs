using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Tomato.MediaPlayer.Views;
using WordRemember.Domain;
namespace Tomato.MediaPlayer
{
    [Module(ModuleName = ModuleNames.MEDIA_PLAYER)]
    public class MediaPlayerModule : IModule
    {
        private readonly RegionManager mRegionManager;
        private readonly IUnityContainer mContainer;
        public MediaPlayerModule(RegionManager regionManager, IUnityContainer container)
        {
            mRegionManager = regionManager;
            mContainer = container;
        }

        public void Initialize()
        {
            mRegionManager.RegisterViewWithRegion(RegionNames.MEDIA_PLAYER_REGION, typeof( MediaPlayerView));

        }
    }
}
