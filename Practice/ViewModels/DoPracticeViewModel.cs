using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Common;
using Contract;
using Models;
using Practice.Views;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using WordRemember.Domain;
namespace Practice.ViewModels
{
    public class DoPracticeViewModel : ViewModelBase, INavigationAware
    {
        #region Fields
        private readonly DispatcherTimer Timer = new DispatcherTimer();
        private readonly IRegionManager mRegionManager;
        private readonly IAudioService mAudioService;
        private readonly IPractiseService mPractiseService;
        private string mUserName;
        private string mBook;
        private DateTime mStartTime;
        private CollectionViewSource mWordView;
        private ICommand mShowNextWordCommand;
        private ObservableCollection<UIWord> mWords = new ObservableCollection<UIWord>();
        private List<WordPractise> mPractises = new List<WordPractise>();
        private bool mIsDisplayChinese = true;
        private ICommand mGoBackCommand;
        private int mTotalSeconds;
        private float mWordCountInMinute;
        #endregion
        #region Properties
        public int TotalSeconds {
            get {
                return mTotalSeconds;
            }
            set {
                mTotalSeconds = value;
                OnPropertyChanged();
            }
        }
        public float WordCountInMinute {
            get {
                return mWordCountInMinute;
            }
            set {
                mWordCountInMinute = value;
                OnPropertyChanged();
            }
        }
        public InteractionRequest<Notification> NotifyDoneRequest {
            get;
        } = new InteractionRequest<Notification>();
        public InteractionRequest<Notification> ClearInputTextRequest {
            get;
        } = new InteractionRequest<Notification>();
        public InteractionRequest<Notification> PlayWordRequest {
            get;
        } = new InteractionRequest<Notification>();
        public string UserName {
            get {
                return mUserName;
            }
            set {
                mUserName = value;
                OnPropertyChanged();
            }
        }
        public bool IsDisplayChinese {
            get {
                return mIsDisplayChinese;
            }
            set {
                mIsDisplayChinese = value;
                OnPropertyChanged();
            }
        }
        public string Book {
            get {
                return mBook;
            }
            set {
                mBook = value;
                OnPropertyChanged();
            }
        }
        public UIWord CurrentWord => WordView.View.CurrentItem as UIWord;
        private ObservableCollection<UIWord> Words {
            get {
                return mWords ?? (mWords = new ObservableCollection<UIWord>());
            }
        }
        public CollectionViewSource WordView {
            get {
                if(mWordView == null) {
                    mWordView = new CollectionViewSource {Source = Words};
                }
                return mWordView;
            }
        }
        public int WordsCount {
            get {
                return Words.Count;
            }
        }
        public int CurrentPosition {
            get {
                return WordView.View.CurrentPosition + 1;
            }
        }
        #endregion
        #region Mothds
        private void GoBack () {
            mRegionManager.Regions[RegionNames.PRACTICE_REGION].RequestNavigate(typeof(PracticeSetupView).FullName);
        }
        public DoPracticeViewModel (IRegionManager regionManager, IAudioService audioService,
            IPractiseService practiseService) {
            mRegionManager = regionManager;
            mAudioService = audioService;
            mPractiseService = practiseService;
            Timer.Tick += TimerOnTick;
            Timer.Interval = new TimeSpan(0, 0, 0, 1);
        }
        private void TimerOnTick (object sender, EventArgs eventArgs) {
            var span = DateTime.Now.Subtract(mStartTime);
            var x = span.TotalSeconds;
            var y = 60 / x;
            this.WordCountInMinute =  (float)(this.WordView.View.CurrentPosition * y);
            Dispatcher.CurrentDispatcher.BeginInvoke(
                new Action(() => { TotalSeconds = Convert.ToInt32(span.TotalSeconds); }));
        }
        public void OnNavigatedTo (NavigationContext navigationContext) {
            Words.Clear();
            mPractises.Clear();
            mStartTime = DateTime.Now;
            TotalSeconds = 0;
            WordCountInMinute = 0;
            Timer.Start();

            UserName = (string)navigationContext.Parameters["UserName"];
            Book = ((UIBook)navigationContext.Parameters["Book"]).Name;
            Modules = (string[])navigationContext.Parameters["Modules"];
            var words1 = navigationContext.Parameters["Words"] as List<UIWord>;
            while(words1.Count > 0) {
                var dan = new Random();
                var index = dan.Next(0, words1.Count);
                var item = words1[index];
                words1.Remove(item);
                Words.Add(item);
            }
            WordView.View.MoveCurrentToFirst();
            PlayWord();
        }
        public string[] Modules {
            get;
            set;
        }
        public bool IsNavigationTarget (NavigationContext navigationContext) {
            return true;
        }
        public void OnNavigatedFrom (NavigationContext navigationContext) {}
        #endregion
        #region Commands
        public ICommand ShowNextWordCommand {
            get {
                return mShowNextWordCommand ?? (mShowNextWordCommand = new DelegateCommand<string>(s => {
                           if(CurrentWord == null)
                               return;
                           if(string.IsNullOrEmpty(s))
                               return;

                           if(!s.Equals(CurrentWord.Name, StringComparison.InvariantCulture)) {
                               IsDisplayChinese = false;
                               Words.Add(CurrentWord);
                               if(mPractises.All(a => a.Word != CurrentWord.Name)) {
                                   mPractises.Add(new WordPractise() {Word = CurrentWord.Name, IsSucceed = false});
                               }
                           } else {
                               if(mPractises.All(a => a.Word != CurrentWord.Name)) {
                                   mPractises.Add(new WordPractise() {Word = CurrentWord.Name, IsSucceed = true});
                               }
                               if(WordView.View.MoveCurrentToNext()) {
                                   IsDisplayChinese = true;
                                   PlayWord();
                               }
                           }
                           ClearInputTextRequest?.Raise(new Notification());
                           if(CurrentWord == null) {
                               TimeSpan span = DateTime.Now.Subtract(mStartTime);
                               Timer.Stop();
                               mPractiseService.SavePractise(UserName, Book, Modules, mPractises,
                                   Convert.ToInt32(span.TotalSeconds), WordCountInMinute);
                               NotifyDoneRequest.Raise(new Notification {Title = "GOOD", Content = "你已完成练习!"},
                                   n => GoBack());
                           }
                       })); 
            }
        }
        public ICommand GoBackCommand {
            get {
                return mGoBackCommand ?? (mGoBackCommand = new DelegateCommand(() => { GoBack(); }));
            }
        }
        private void PlayWord () {
            if(CurrentWord == null)
                return;
            OnPropertyChanged(nameof(CurrentWord));
            OnPropertyChanged(nameof(WordsCount));
            OnPropertyChanged(nameof(CurrentPosition));
            PlayWordRequest.Raise(new Notification());
            mAudioService.PlaySound(CurrentWord.Name);
        }
        #endregion
    }
}
