using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Common;
using Contract;
using log4net;
using Models;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using WordRemember.Domain;
namespace Tomato.Exercise.ViewModels
{
    public class PlanViewModel : ViewModelBase
    {
        #region Fields
        private readonly IBookService mBookService; 
        private readonly IDocumentService mDocumentService;
        private readonly ILog mLog;
        private readonly IReviewPlanService mPlanService;
        private bool mIsEnabled;
        private readonly object mLockObject = new object();
        private readonly object mPlanLock = new object();
        private ObservableCollection<UIPlan> mPlans;
        private ICommand mSaveCommand;
        private readonly string mUser = "Xinao";
        private ICommand mViewBookCommand;
        #endregion
        #region Properties
        public bool IsEnabled
        {
            get { return mIsEnabled; }
            set
            {
                mIsEnabled = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UIPlan> Plans => mPlans ?? (mPlans = new ObservableCollection<UIPlan>());
        public InteractionRequest<INotification> NotificationRequest { get; } = new InteractionRequest<INotification>();
        public ICommand ViewBookCommand
        {
            get
            {
                return mViewBookCommand ?? (mViewBookCommand = new DelegateCommand<UIPlan>(plan =>
                       {
                           try
                           {
                               Process.Start($"{SystemFileNames.BOOK_PATH}\\{plan.Book}");
                           }
                           catch(Exception) {}
                       }));
            }
        }
        public ICommand SaveCommand
        {
            get { return mSaveCommand ?? (mSaveCommand = new DelegateCommand(Save)); }
        }
        public ICommand GotoBookDirectoryCommand { get; } = new DelegateCommand(() =>
        {
            try
            {
                Process.Start($"{SystemFileNames.BOOK_PATH}\\");
            }
            catch(Exception) {}
        });
        #endregion
        #region Constructors
        public PlanViewModel(ILog log, IBookService bookService, IReviewPlanService planService,
            IDocumentService documentService)
        {
            mLog = log;
            mBookService = bookService;
            mPlanService = planService;
            mDocumentService = documentService;
            BindingOperations.EnableCollectionSynchronization(Plans, mPlanLock);
            Init();
            WatcherBooks();
        }
        #endregion
        #region Private or Protect Methods
        private async Task Init()
        {
            IsEnabled = false;
            await Task.Factory.StartNew(() =>
            {
                lock(mLockObject)
                {
                    Plans.Clear();
                    var books = mBookService.GetBooks();
                    var plans = mPlanService.GetReviewPlanDatas(mUser);
                    foreach(var book in books)
                    {
                        var points = mDocumentService.GetBookPoints(book.Name);
                        var plan = new UIPlan
                        {
                            Book = book.Name,
                            BookTitle = book.Title,
                            PointsCount = points.Count
                        };
                        Plans.Add(plan);
                        var savedPlan = plans.FirstOrDefault(a => a.Book.Equals(plan.Book));
                        if(savedPlan != null)
                        {
                            plan.Days = savedPlan.Days;
                            plan.StartDate = savedPlan.StartDate;
                        }
                    }
                }
            });
            IsEnabled = true;
        }
        private void Save()
        {
            var plans = new List<ReviewPlanData>();
            foreach(var p in Plans)
            {
                var plan = new ReviewPlanData
                {
                    Book = p.Book,
                    Days = p.Days,
                    StartDate = p.StartDate,
                    User = mUser
                };
                plans.Add(plan);
            }
            mPlanService.SaveReviewPlanData(plans);
            NotificationRequest.Raise(new Notification
            {
                Title = "提示",
                Content = "保存成功！"
            });
        }
        private void WatcherBooks()
        {
            var watcher = new FileSystemWatcher( SystemFileNames.BOOK_PATH);
            watcher.Filter = "*.docx";
            watcher.Changed += WatcherOnChanged;
            watcher.Deleted += WatcherOnChanged;
            watcher.Renamed += WatcherOnChanged;
            watcher.Created += WatcherOnChanged;
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = false;
            watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName |  
                                   NotifyFilters.LastWrite   ;
            
        }
        private async void WatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            await Init();
        }
        #endregion
        public class UIPlan : BindableBase
        {
            #region Fields
            private string mBook;
            private string mBookTitle;
            private int mDays;
            private int mPointsCount;
            private DateTime? mStartDate;
            #endregion
            #region Properties
            public string BookTitle
            {
                get { return mBookTitle; }
                set
                {
                    mBookTitle = value;
                    OnPropertyChanged();
                }
            }
            public string Book
            {
                get { return mBook; }
                set
                {
                    mBook = value;
                    OnPropertyChanged();
                }
            }
            public int Days
            {
                get { return mDays; }
                set
                {
                    mDays = value;
                    OnPropertyChanged();
                }
            }
            public DateTime? StartDate
            {
                get { return mStartDate; }
                set
                {
                    mStartDate = value;
                    OnPropertyChanged();
                }
            }
            public int PointsCount
            {
                get { return mPointsCount; }
                set
                {
                    mPointsCount = value;
                    OnPropertyChanged();
                }
            }
            #endregion
        }
    }
}