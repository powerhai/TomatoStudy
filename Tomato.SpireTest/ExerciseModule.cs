using Prism.Modularity;
using Prism.Regions;
using Tomato.SpireTest.Views;
using WordRemember.Domain;
namespace Tomato.SpireTest
{
    [Module(ModuleName ="test")]
    [ModuleDependency(ModuleNames.SERVICES_MODULE)]
    public class ExerciseModule : IModule
    {
        private readonly IRegionManager mRegionManager;

        public ExerciseModule(IRegionManager regionManager)
        {
            mRegionManager = regionManager;
        }
        public void Initialize()
        {
            mRegionManager.RegisterViewWithRegion(RegionNames.CONTENT_REGION, typeof(DocView)); 
        }
    }
}
