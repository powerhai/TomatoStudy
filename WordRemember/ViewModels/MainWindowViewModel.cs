using Prism.Mvvm;

namespace WordRemember.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string mTitle = "蕃茄学习";
        public string Title {
            get { return mTitle; }
            set { SetProperty(ref mTitle, value); }
        }
 
    }
}
