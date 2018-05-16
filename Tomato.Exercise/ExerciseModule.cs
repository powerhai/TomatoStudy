using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Modularity;
using Prism.Regions;
using Tomato.Exercise.Views;
using WordRemember.Domain;
namespace Tomato.Exercise
{
    [Module(ModuleName = ModuleNames.TOMATO_EXERCISE)]
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
           mRegionManager.RegisterViewWithRegion(RegionNames.CONTENT_REGION, typeof(ContainerView)); 
           mRegionManager.RegisterViewWithRegion(RegionNames.EXERCISE_REGION, typeof(ExerciseView)); 
           mRegionManager.RegisterViewWithRegion(RegionNames.EXERCISE_REGION, typeof(PlanView));
           mRegionManager.RegisterViewWithRegion(RegionNames.EXERCISE_REGION, typeof(HistoryView));
        }
    }
}
