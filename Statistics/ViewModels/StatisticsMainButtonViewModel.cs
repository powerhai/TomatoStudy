using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Contract;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Statistics.Views;
using WordRemember.Domain;
namespace Statistics.ViewModels
{
    public class StatisticsMainButtonViewModel : BindableBase
    {
        private readonly IRegionManager mRegionManager;
        private ICommand mGoPracticesViewCommand;
        public string Title {
            get {
                return "统计";
            }
        }
        public StatisticsMainButtonViewModel (IRegionManager regionManager,IWordManager wordManager) {
            mRegionManager = regionManager;
        }
        public ICommand GoPracticesViewCommand {
            get {
                return mGoPracticesViewCommand ??( mGoPracticesViewCommand =  new DelegateCommand(() => {
                           mRegionManager.Regions[RegionNames.CONTENT_REGION].RequestNavigate(typeof(PracticesView).FullName);
                       }));
            } 
        }
    }
}
