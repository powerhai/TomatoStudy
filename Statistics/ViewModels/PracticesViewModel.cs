using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Common;
using Contract;
using Models;
using Prism.Commands;
using Prism.Regions;
using Prism.Mvvm;

namespace Statistics.ViewModels {
    public class PracticesViewModel : ViewModelBase  {
        private readonly IRegionManager mRegionManager;
        private readonly IPractiseService mPractiseService;
        public PracticesViewModel(IRegionManager regionManager, IPractiseService practiseService ) {
              
            mRegionManager = regionManager;
            mPractiseService = practiseService;
            Users.Add("Haiser");
            Users.Add("Xinao");
            Users.Add("Xinrui");
            Init();
        }
        private void Init () {
             
        }
        public ObservableCollection<WordPractiseData> ErrorWords {
            get; 
        } = new ObservableCollection<WordPractiseData>();
        public ObservableCollection<string> Users { get; } = new ObservableCollection<string>();
        private ObservableCollection<PracticeData> mPractices = new ObservableCollection<PracticeData>();
        private CollectionViewSource mPracticeSource;
        private ICommand mShowUserDetailsCommand;
        public CollectionViewSource PracticeSource {
            get {
                return mPracticeSource??(mPracticeSource = new CollectionViewSource() { Source = mPractices });
            } 
        }
        public ICommand ShowUserDetailsCommand {
            get {
                return mShowUserDetailsCommand??(mShowUserDetailsCommand = new DelegateCommand<string>(user => {
                           mPractices.Clear();
                           var pras = mPractiseService.GetPractices(user);
                           mPractices.AddRange(pras);
                            ErrorWords.Clear();
                           ErrorWords.AddRange(mPractiseService.GetWordPractiseDatas(user));
                       }));
            } 
        }
    }
    
}
