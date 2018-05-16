using Prism.Modularity;
using Prism.Regions;
using System;
using Practice.Views;
using WordRemember.Domain;
namespace Practice
{
    [Module(ModuleName=ModuleNames.PRACTICE_MODULE)]
    [ModuleDependency(ModuleNames.SERVICES_MODULE)]
    public class PracticeModule : IModule
    {
        private readonly IRegionManager mRegionManager;

        public PracticeModule(IRegionManager regionManager) {
            mRegionManager = regionManager;
        }

        public void Initialize() {
            mRegionManager.RegisterViewWithRegion(RegionNames.MAIN_MENU_REGION, typeof(PracticeMainButton));
            mRegionManager.RegisterViewWithRegion(RegionNames.CONTENT_REGION, typeof(PracticeWorkspaceView));
            
            mRegionManager.RegisterViewWithRegion(RegionNames.PRACTICE_REGION, typeof(PracticeSetupView));  
            mRegionManager.RegisterViewWithRegion(RegionNames.PRACTICE_REGION, typeof(DoPracticeView));

        }
        
    }
}