using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Common;
using Contract;
using Prism.Mvvm;
using WordRemember.Domain;
namespace Tomato.Exercise.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private string mUser = "Xinao";
        private readonly IExreciseHistoryService mHistoryService;
        private ObservableCollection<UIHistory> mHistories = new ObservableCollection<UIHistory>();
        private ListCollectionView mDataView;
        public ListCollectionView DataView
        {
            get
            {
                if(mDataView == null)
                {
                    mDataView = new ListCollectionView(mHistories);
                    mDataView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(UIHistory.ReviewDate)));
                    mDataView.SortDescriptions.Add(new SortDescription(nameof(UIHistory.ReviewDate), ListSortDirection.Descending));
                }
                return mDataView  ;
                
            } 
        }
        private object mLockObject = new object();
        public HistoryViewModel(IExreciseHistoryService historyService)
        {
            mHistoryService = historyService;
            BindingOperations.EnableCollectionSynchronization(mHistories, mLockObject);
            WatcherBooks();
            Init();
        }
        private void WatcherBooks()
        {
            //var watcher = new FileSystemWatcher($"{SystemFileNames.BOOK_PATH}\\{SystemFileNames.EXRECISE_DAYS_FILE_PATH}");
            //watcher.Filter = "*.docx";
            //watcher.Changed += WatcherOnChanged;
             
            //watcher.EnableRaisingEvents = true;
            //watcher.IncludeSubdirectories = false;
            //watcher.NotifyFilter =  NotifyFilters.LastWrite;
        }
        private async void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            await Init();
        }
        private async Task Init()
        { 
            await Task.Factory.StartNew(() =>
            {
                var list = mHistoryService.GetDaysExrecises(mUser);
                foreach(var item in list)
                {
                    var history = mHistories.FirstOrDefault(a => a.Guid.Equals(item.Guid));
                    if(history == null)
                    {
                        history = new UIHistory()
                        {
                            Book = item.Book,
                            BookTitle = item.BookTitle,
                            Guid = item.Guid,
                            Module = item.Module,
                            Unit = item.Unit,
                            ReviewCount = item.ReviewCount,
                            ReviewDate = item.ReviewDate.ToString("yyyy-MM-dd"),
                            User = item.User
                        };
                        mHistories.Add(history);
                    }
                    else
                    {
                        history.ReviewCount = item.ReviewCount;
                    }
                }
                
            });
        }

        public class UIHistory : BindableBase
        {
            private int mReviewCount;
            public string User { get; set; }
            public string Book { get; set; }
            public string BookTitle { get; set; }
            public string Module { get; set; }
            public string Unit { get; set; }
            public string ReviewDate { get; set; }
            public string Guid { get; set; }
            public int ReviewCount
            {
                get { return mReviewCount; }
                set
                {
                    mReviewCount = value; 
                    OnPropertyChanged();
                }
            }
        }
    }
}