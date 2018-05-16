using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using Common;
using Contract;
using Models;
using Practice.Views;
using Prism.Commands;
using Prism.Regions;
using WordRemember.Domain;
namespace Practice.ViewModels {
    public class PracticeSetupViewModel : ViewModelBase, INavigationAware {
        private readonly IRegionManager mRegionManager;
        private readonly IWordManager mWordManager;
        private readonly IPractiseService mPractiseService;
        private List<UIModule> mSelectedModules;
        private string mUserName;
        private UIBook mBook;
        private bool mOnlyErrorWords = false;
        public string UserName {
            get {
                return mUserName;
            }
            set {
                mUserName = value;
                OnPropertyChanged();
            }
        }
        public UIBook Book {
            get {
                return mBook;
            }
            set {
                mBook = value;
                OnPropertyChanged();
            }
        }
        public bool OnlyErrorWords {
            get {
                return mOnlyErrorWords;
            }
            set {
                mOnlyErrorWords = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Users {  get;  } = new ObservableCollection<string>();
        public ObservableCollection<UIBook> Books { get; }= new ObservableCollection<UIBook>();
        public List<UIModule> SelectedModules {
            get {
                return mSelectedModules;
            }
            set {
                mSelectedModules = value;
                OnPropertyChanged();
            }
        }
        public PracticeSetupViewModel (IRegionManager regionManager,IWordManager wordManager, IPractiseService practiseService) {
            mRegionManager = regionManager;
            mWordManager = wordManager;
            mPractiseService = practiseService;
            Users.Add("Haiser");
            Users.Add("Xinao");
            Users.Add("Xinrui");
            Init();
        }
        private void Init () {
            Books.AddRange(mWordManager.GetBooks());
        }
        public ICommand StartPracticeCommand {
            get {
                return new DelegateCommand<IEnumerable<object>>((models) => {
                    var ms = models.Cast<UIModule>();
                    var words = ms.SelectMany(a => a.Words).ToList();
                    if(OnlyErrorWords) {
                       var errWords =  mPractiseService.GetWordPractiseDatas(UserName).Where(a => a.Failed > 0).Select(a=>a.Word).ToArray();
                       var endWords = new List<UIWord>();
                        foreach(var w in words) {
                            if(errWords.Contains(w.Name))
                                endWords.Add(w);
                        }
                        words = endWords;
                    }
                    var modules = ms.Select(a => a.Name).ToArray();
                    var navParms = new NavigationParameters();
                    navParms.Add("UserName", UserName);
                    navParms.Add("Book", Book);
                    navParms.Add("Words", words); 
                    navParms.Add("Modules", modules);
                    mRegionManager.Regions[RegionNames.PRACTICE_REGION].RequestNavigate( typeof(DoPracticeView).FullName,  navParms);
                });
            }
        }
        public void OnNavigatedTo (NavigationContext navigationContext) {
             
        }
        public bool IsNavigationTarget (NavigationContext navigationContext) {
            return true; 
        }
        public void OnNavigatedFrom (NavigationContext navigationContext) {
             
        }
    }
}
