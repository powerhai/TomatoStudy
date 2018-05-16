using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WordRemember.Domain;
using Practice.Views;

namespace Practice.ViewModels {
    public class PracticeMainButtonViewModel : ViewModelBase {
        private readonly IRegionManager mRegionManager;
        public PracticeMainButtonViewModel (IRegionManager regionManager) {
            mRegionManager = regionManager;
        }
        public ICommand GoCommand {
            get {
                return new DelegateCommand(() => {
                    mRegionManager.Regions[RegionNames.CONTENT_REGION].RequestNavigate( typeof(PracticeWorkspaceView).FullName );
                });
            }
        }
    }
}
