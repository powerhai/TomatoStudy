using Prism.Modularity;
using Prism.Regions;
using System;
using Statistics.Views;
using WordRemember.Domain;
namespace Statistics
{
    [Module(ModuleName= ModuleNames.STATISTICS_MODULE)]
    [ModuleDependency(ModuleNames.SERVICES_MODULE)]
    public class StatisticsModule : IModule
    {
        readonly IRegionManager _regionManager;

        public StatisticsModule(RegionManager regionManager) {
            _regionManager = regionManager;
        }

        public void Initialize() {
            _regionManager.RegisterViewWithRegion(RegionNames.MAIN_MENU_REGION, typeof(StatisticsMainButton));
            _regionManager.RegisterViewWithRegion(RegionNames.CONTENT_REGION, typeof(PracticesView));
        }
    }
}