using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using Contract;
using Models;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Managerment.ViewModels
{
    public class WordsManagermentViewModel : ViewModelBase {
        private readonly IWordManager mWordManager;
        private UIModule mCurrentModule;
        public string Title => "词汇管理";
        public ViewData Data { get; } = new ViewData();
             
        public WordsManagermentViewModel (IWordManager wordManager) {
            mWordManager = wordManager;
            LoadData();
        }

        private void LoadData () {
            var books = mWordManager.GetBooks();
            if(books != null) {
                Data.Books.AddRange(books);
            }
        }
        private void SaveData () {
            var books = Data.Books.ToList();
            mWordManager.SaveBooks(books);
        }
        public ICommand EditWordCommand => new  DelegateCommand<UIWord>(word => {
            if(word == null) {
                this.NotificationRequest.Raise(new Notification() { Title = "提示", Content = "没有选中单词" });
                return;
            }
            this.EditWordConfirmationRequest.Raise(new WordEditNotification() {
                Title ="编辑单词", Name = word.Name, Sentence = word.Sentence, SplittedText = word.SplittedText,   Symbol = word.Symbol , KeyGuid = word.KeyGuid, Meaning = word.Meaning, Explains = word.Explains, SymbolUs = word.SymbolUs
            }, UpdateWordResponse);
        });
        private void UpdateWordResponse (WordEditNotification obj) {
             if(obj.Confirmed == false)
                return;
            var word = CurrentModule.Words.FirstOrDefault(a => a.KeyGuid == obj.KeyGuid);
            if(word != null) {
                word.Name = obj.Name;
                word.Sentence = obj.Sentence;
              
                word.SplittedText = obj.SplittedText;
                word.Symbol = obj.Symbol;
                word.Meaning = obj.Meaning;
                word.Explains = obj.Explains;
                word.SymbolUs = obj.SymbolUs;
            }
        }
        public ICommand AddWordCommand => new DelegateCommand(() => {
            if(CurrentModule == null) {
                this.NotificationRequest.Raise(new Notification() {Title ="提示", Content="没有选中Module"});
                return;
            }
            this.AddWordConfirmationRequest.Raise(new WordEditNotification { Title = "添加单词" }, OnAddWordResponse);
             
        });
        private void OnAddWordResponse (WordEditNotification obj) {
            if(obj.Confirmed) {
                if(CurrentModule != null) {
                    var word = new UIWord() {
                        Name = obj.Name,
                        SplittedText = obj.SplittedText,
                        Symbol = obj.Symbol,
                        Sentence = obj.Sentence,
                        Meaning = obj.Meaning,
                      
                        KeyGuid = obj.KeyGuid,
                        Explains = obj.Explains,
                        SymbolUs = obj.SymbolUs
                    }; 
                    CurrentModule.Words.Add(word);
                }
            }
        }
        public ICommand SaveCommand => new DelegateCommand(() => { SaveData(); });

        public InteractionRequest<INotification> NotificationRequest {
            get;
        } = new InteractionRequest<INotification>();
        public InteractionRequest<WordEditNotification> AddWordConfirmationRequest {
            get;
        } = new InteractionRequest<WordEditNotification>();

        public InteractionRequest<WordEditNotification> EditWordConfirmationRequest {
            get;
        } = new InteractionRequest<WordEditNotification>();

        public UIModule CurrentModule {
            get {
                return mCurrentModule;
            }
            set {
                mCurrentModule = value;
                OnPropertyChanged();
            }
        }
    }
}
