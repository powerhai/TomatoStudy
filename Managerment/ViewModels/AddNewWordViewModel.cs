using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using Common;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
namespace Managerment.ViewModels
{
    public class AddNewWordViewModel : ViewModelBase, IInteractionRequestAware
    {
        private WordEditNotification mData;
        public WordEditNotification Data {
            get {
                return mData;
            }
            set {
                mData = value;
                OnPropertyChanged();
            }
        }
        public ICommand SearchWordCommand {
            get {
                return new DelegateCommand<string>((text) => { SearchWord(text); });
            }
        }
        private CancellationTokenSource mCancellationToken;
        private readonly object mLockObject = new object();
        private const string GET_WORD_URL =
            "http://fanyi.youdao.com/openapi.do?keyfrom=youdao111&key=60638690&type=data&doctype=xml&version=1.1&q={0}";
        private void SearchWord (string word) {
            lock(mLockObject) {
                mCancellationToken?.Cancel();
                mCancellationToken = new CancellationTokenSource(); 
                Task.Run(async () => {
                    try {
                        var client = new HttpClient();
                        var url = string.Format(GET_WORD_URL, word);
                        var str = await client.GetStreamAsync(url);
                        var doc = XDocument.Load(str);
                        var symbol = doc.Root.Element("basic").Element("phonetic").Value;
                        var splittedText = word;
                        var meanings = doc.Root.Element("translation").Element("paragraph").Value;
                        var symbolUs = doc.Root.Element("basic").Element("us-phonetic").Value;
                        var explains = string.Join("\r\n",
                            doc.Root?.Element("basic")?.Element("explains")?.Elements("ex")?.Select(a => a.Value).ToArray());
                       
                        Dispatcher.CurrentDispatcher.Invoke(() => {
                            Data.Symbol = symbol;
                            Data.SplittedText = splittedText;
                            Data.Meaning = meanings;
                            Data.Explains = explains;
                            Data.SymbolUs = symbolUs;
                        });
                    }
                    catch(Exception e) {
                         
                    }
 
                }, mCancellationToken.Token);
            }
        }
        public ICommand SaveCommand {
            get {
                return new DelegateCommand(() => {
                    Data.Confirmed = !string.IsNullOrEmpty(Data.Name);
                    FinishInteraction();
            });}
        }
        public ICommand CancelCommand {
            get {
                return new DelegateCommand(() => {
                    Data.Confirmed = false;
                    FinishInteraction();
                });
            }
        } 
        public INotification Notification {
            get {
                return Data;
            }
            set {
                Data = value as WordEditNotification;
                OnPropertyChanged();
            }
        }
        public Action FinishInteraction {
            get;
            set;
        }
    }
}
