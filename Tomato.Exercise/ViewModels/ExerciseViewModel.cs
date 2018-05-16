using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Common;
using Contract;
using Contract.Events;
using log4net;
using Models;
using Prism.Commands;
using Prism.Events;
namespace Tomato.Exercise.ViewModels
{
    public class ExerciseViewModel : ViewModelBase
    {
        enum  ReviewMode
        {
            ReviewToday,
            ReviewAll
        }
        #region Fields
        private ReviewMode mReviewMode = ReviewMode.ReviewAll;
        private readonly ILog mLog;
        private readonly IBookService mBookService;
        private readonly IDocumentService mDocumentService;
        private readonly IEventAggregator mEventAggregator;
        private readonly IExreciseHistoryService mExreciseHistoryService;
        private readonly IReviewPlanService mPlanService;
        private List<BookPoint> mReviewPoints = new List<BookPoint>();
        private FixedDocumentSequence mCurrentDocument;
        private ICommand mEditBookCommand;
        private bool mIsEnabled; 
        private bool mIsOver; 
        private ICommand mRefreshCommand;
        private ICommand mShowNextDocumentCommand;
        private ListCollectionView mBookView;
        private ListCollectionView mPointView;
        private ICommand mReviewTodayCommand;
        #endregion
        #region Properties
        public int PointCount => mReviewPoints.Count;
        public string User => "Xinao";
        public bool IsOver
        {
            get { return mIsOver; }
            set
            {
                mIsOver = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return mIsEnabled; }
            set
            {
                mIsEnabled = value;
                OnPropertyChanged();
            }
        }
        public ListCollectionView PointView => mPointView ?? (mPointView = new ListCollectionView(mReviewPoints));
        public ObservableCollection<Book> Books { get; } = new ObservableCollection<Book>();
        public FixedDocumentSequence CurrentDocument
        {
            get { return mCurrentDocument; }
            set
            {
                mCurrentDocument = value;
                OnPropertyChanged();
            }
        }
        public ICommand ShowNextDocumentCommand
        {
            get
            {
                return mShowNextDocumentCommand ?? (mShowNextDocumentCommand = new DelegateCommand<string>(async easy =>
                       {

                           mEventAggregator.GetEvent<StopMediaPlayingEvent>().Publish();
                           var isEasy = Convert.ToBoolean(easy);
                           var cur = PointView.CurrentItem as BookPoint;
                           mExreciseHistoryService.SaveExrecise(User, cur.Book, cur.BookTitle, cur.Module, cur.Unit);
                           if(!PointView.MoveCurrentToNext())
                           {
                               IsOver = true;
                               return;
                           }
                           IsEnabled = false;
                           await LoadCurrentDocument();
                           IsEnabled = true;
                       }));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return mRefreshCommand ?? (mRefreshCommand = new DelegateCommand(async () =>
                       { 
                           mReviewMode = ReviewMode.ReviewAll;
                           await Init();
                       }));
            }
        }

        public ICommand EditBookCommand
        {
            get
            {
                return mEditBookCommand ?? (mEditBookCommand = new DelegateCommand<Book>(book =>
                       {
                           try
                           {
                               Process.Start($"Books\\{book.Name}");
                           }
                           catch(Exception) {}
                       }));
            }
        }
        public ICommand ReviewTodayCommand
        {
            get { return mReviewTodayCommand ?? (mReviewTodayCommand = new DelegateCommand(async () =>
                         {
                             mReviewMode = ReviewMode.ReviewToday;
                             await Init();
                         })); } 
        }
        #endregion
        #region Constructors

        public ExerciseViewModel(ILog log, IBookService bookService, IDocumentService documentService,
            IEventAggregator eventAggregator, IExreciseHistoryService exreciseHistoryService , IReviewPlanService planService)
        {
            mLog = log;
            mBookService = bookService;
            mDocumentService = documentService;
            mEventAggregator = eventAggregator;
            mExreciseHistoryService = exreciseHistoryService;
            mPlanService = planService;
            mLog.Info("ExerciseViewModel has been loaded.");
            Init();
        }
        #endregion
        #region Private or Protect Methods
        private async Task Init()
        {
            IsEnabled = false;
            mCurrentDocument = null;  
            IsOver = false;
            if(mReviewMode == ReviewMode.ReviewAll)
                await InitByReviewAll();
            else
            {
                await InitByReviewToday();
            }
        }
        private async Task InitByReviewToday()
        {
            var histories = mExreciseHistoryService.GetTodayExrecises(User);

            await Task.Run(() =>
            { 
                var books = mBookService.GetBooks();
                mReviewPoints.Clear(); 
                foreach (var book in books) {
                    var points = mDocumentService.GetBookPoints(book.Name); 
                     
                    foreach (var point in points) {
                        point.BookTitle = book.Title;
                        var history = histories.FirstOrDefault(
                                a =>
                                    (a.User == User) && (a.Book == point.Book) && (a.Module == point.Module) &&
                                    (a.Unit == point.Unit));

                        if (history != null) {
                            mReviewPoints.Add(point);
                        } 
                    } 
                }
            });
            mLog.Info("ExerciseViewModel has been inited.");
            OnPropertyChanged(nameof(PointCount));
            StartStudy();
        }
        private async Task InitByReviewAll()
        {
            var histories = mExreciseHistoryService.GetExrecises(User);

            await Task.Run(() =>
            {
                try {
                    mBookService.GetBooks();
                }
                catch (Exception e) {
                    mLog.Info($"Exception {e}");
                }
                var books = mBookService.GetBooks();
                mReviewPoints.Clear();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Books.Clear();
                    Books.AddRange(books);
                });

                foreach (var book in books) {
                    var points = mDocumentService.GetBookPoints(book.Name);
                    var count = 0;
                    var ablePoints = new List<BookPoint>();
                    foreach (var point in points) {
                        point.BookTitle = book.Title;
                        var history = histories.FirstOrDefault(
                                a =>
                                    (a.User == User) && (a.Book == point.Book) && (a.Module == point.Module) &&
                                    (a.Unit == point.Unit));

                        if (history != null) {
                            var nextDay = history.ReviewDate.AddDays(history.ReviewCount);
                            if (nextDay > DateTime.Now)
                                continue;
                            point.MinuteForTimesup = DateTime.Now.Subtract(nextDay).TotalMinutes;
                        }
                        else {
                            point.MinuteForTimesup = 999999999;
                        }
                        ablePoints.Add(point); 
                    }
                    ablePoints.Sort((a, b) => b.MinuteForTimesup > a.MinuteForTimesup ? 1 : 0);
                    while (count < 10 && count < ablePoints.Count) {
                        mReviewPoints.Add(ablePoints[count]);
                        count++;
                    }
                }
            });
            mLog.Info("ExerciseViewModel has been inited.");
            OnPropertyChanged(nameof(PointCount));
            StartStudy();
        }
        private async Task LoadCurrentDocument()
        {

            mEventAggregator.GetEvent<StopMediaPlayingEvent>().Publish();
            var docc = await Task.Factory.StartNew(() =>
                    {
                        var cur = PointView.CurrentItem as BookPoint;
                        var doc = mDocumentService.GetDocument(cur.Book, cur.Module, cur.Unit);
                        return doc;
                    });
                    CurrentDocument = docc.Document.GetFixedDocumentSequence();
                    PlayMusic(docc.MusicFilePath); 
            
        }

        private List<BookPoint> GetNotReviewedPointsOfBook(List<BookPoint> points, List<ExreciseData> history  )
        {
            var list = new List<BookPoint>();
            foreach(var point in points)
            {
                foreach(var hi in history)
                {
                    if( !(hi.User.Equals(User) && hi.Book.Equals(point.Book) && hi.Module.Equals(point.Module) &&
                       hi.Unit.Equals(point.Unit)))
                    {
                        list.Add(point);
                    }
                }
            } 
            return list;
        }
        private List<BookPoint> GetReviewedPointsOfBook(List<BookPoint> points, List<ExreciseData> history)
        {
            var list = new List<BookPoint>();
            foreach (var point in points) {
                foreach (var hi in history) {
                    if ((hi.User.Equals(User) && hi.Book.Equals(point.Book) && hi.Module.Equals(point.Module) &&
                       hi.Unit.Equals(point.Unit))) {
                        list.Add(point);
                    }
                }
            }
            return list;
        }

        private List<BookPoint> GetTodayPointsOfNotReviewedPointsByBook(string book, List<BookPoint> pointsOfBook )
        {
            var list = new List<BookPoint>();
            var history = mExreciseHistoryService.GetExrecises(User).Where(a => a.Book.Equals(book)).ToList();
            var notReviewedPoints = GetNotReviewedPointsOfBook(pointsOfBook, history);

            if (notReviewedPoints.Count <= 0)
                return list; 

            var plan = mPlanService.GetReviewPlanDatas(User).FirstOrDefault(a=>a.Book.Equals(book));
            
            //如果没有开课数据，返回前10
            if (plan == null || !plan.StartDate.HasValue || plan.Days == 0 || DateTime.Now > plan.StartDate.Value.AddDays(plan.Days))
            {
                return notReviewedPoints.Take(10).ToList();
            }

            //如果还到达计划开课时间，返回空
            if (plan.StartDate.HasValue && DateTime.Now < plan.StartDate.Value)
                return list;
             
            
            var reviewCountToday = mExreciseHistoryService.GetTodayExrecises(User, book).Count;
            var dayNo = plan.StartDate.Value.AddDays(plan.Days).Subtract(DateTime.Now).TotalDays;
            var reviewCount = (notReviewedPoints.Count + reviewCountToday) / dayNo - reviewCountToday;
            if(reviewCount > 0)
            {
                for(var i = 0; i < reviewCount; i++)
                {
                    list.Add( notReviewedPoints[i]);
                }
            }
            else
            {
                var daysForOnePoint = plan.Days / reviewCount;
                if(dayNo % daysForOnePoint == 0)
                {
                    list.Add(notReviewedPoints.First());
                }
            }
            return (list);
        }
        private void PlayMusic(string fileName)
        {
            if(!string.IsNullOrEmpty(fileName))
            {
                var file = new FileInfo($"Books\\Mp3\\{fileName}");
                if(file.Exists)
                    mEventAggregator.GetEvent<PlayMediaEvent>().Publish(file.FullName);
            }
        }
        private async void StartStudy()
        {
            if(PointView.MoveCurrentToFirst())
            {
                await LoadCurrentDocument();                
            }
            else
            {
                IsOver = true;
            }
            IsEnabled = true;
        }
        #endregion
    }
}