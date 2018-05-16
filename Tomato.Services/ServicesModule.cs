using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using WordRemember.Domain;
namespace Tomato.Services
{
    [Module(ModuleName = ModuleNames.TOMATO_SERVICE)]
    public class ServicesModule : IModule
    {
        private readonly RegionManager mRegionManager;
        private readonly IUnityContainer mContainer;
        public ServicesModule(RegionManager regionManager, IUnityContainer container)
        {
            mRegionManager = regionManager;
            mContainer = container;
        }

        public void Initialize()
        {
            mContainer.RegisterType(typeof(IExreciseHistoryService), typeof(ExreciseHistoryService)); 
            mContainer.RegisterType(typeof(IBookService), typeof(BookService)); 
            mContainer.RegisterType(typeof(IDocumentService), typeof(DocumentService));
            mContainer.RegisterType(typeof(IReviewPlanService), typeof(ReviewPlanService));
        }
    }
}
