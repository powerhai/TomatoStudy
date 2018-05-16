using Prism.Modularity;
using Prism.Regions;
using System;
using Managerment.Views;
using WordRemember.Domain;
namespace Managerment
{
    [Module(ModuleName=ModuleNames.MANAGERMENT_MODULE)]
    [ModuleDependency(ModuleNames.SERVICES_MODULE)]
    public class ManagermentModule : IModule
    {
        IRegionManager _regionManager;

        public ManagermentModule(RegionManager regionManager) {
            _regionManager = regionManager;
        }

        public void Initialize() {
            _regionManager.RegisterViewWithRegion(RegionNames.MAIN_MENU_REGION, typeof(ManagermentMainButton));
            _regionManager.RegisterViewWithRegion(RegionNames.CONTENT_REGION, typeof(WordsManagermentView));
            //_regionManager.RegisterViewWithRegion(RegionNames.CONTENT_REGION, typeof(AddNewWordView));
        }
    }
}