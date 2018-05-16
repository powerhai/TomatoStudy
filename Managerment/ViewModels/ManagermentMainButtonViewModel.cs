using System.Windows.Input;
using Managerment.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WordRemember.Domain;
namespace Managerment.ViewModels
{
    public class ManagermentMainButtonViewModel : BindableBase
    {
        private readonly IRegionManager mRegionManager;
        public ManagermentMainButtonViewModel (IRegionManager regionManager) {
            mRegionManager = regionManager;
        }
        public string Title => "管理";
        public ICommand GotoMgtCommand {
            get {
                return
                    new DelegateCommand(
                        () => {
                            mRegionManager.RequestNavigate(RegionNames.CONTENT_REGION,
                                typeof(WordsManagermentView).FullName);
                        });
            }
        }
    }
}
