using Contract;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using WordRemember.Domain;
namespace Services
{
    [Module(ModuleName = ModuleNames.SERVICES_MODULE)]
    public class ServicesModule : IModule {
        private readonly RegionManager mRegionManager;
        private readonly IUnityContainer mContainer;
        public ServicesModule(RegionManager regionManager, IUnityContainer container) {
            mRegionManager = regionManager;
            mContainer = container;
        }
        
        public void Initialize() {
            mContainer.RegisterType(typeof(IWordManager), typeof(WordManager));
            mContainer.RegisterType(typeof(IAudioService), typeof(AudioService));
            mContainer.RegisterType(typeof(IPractiseService), typeof(PractiseService));
        }
    }
}