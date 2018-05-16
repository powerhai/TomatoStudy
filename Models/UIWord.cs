using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Common;
namespace Models
{
    public class UIWord : NotificationObject , IComparable {
        private string mName;
        private string mSplittedText;
        private string mSymbol;
        private string mSentence; 
        private string mMeaning;
        private string mExplains;
        private string mSymbolUs;
        public string Name {
            get {
                return mName;
            }
            set {
                mName = value;
                OnPropertyChanged();
            }
        }
        public string SplittedText {
            get {
                return mSplittedText;
            }
            set {
                mSplittedText = value;
                OnPropertyChanged();
            }
        }
        public string Symbol {
            get {
                return mSymbol;
            }
            set {
                mSymbol = value;
                OnPropertyChanged();
            }
        }
        public string SymbolUs {
            get {
                return mSymbolUs;
            }
            set {
                mSymbolUs = value;
                OnPropertyChanged();
            }
        }
        public string Sentence {
            get {
                return mSentence;
            }
            set {
                mSentence = value;
                OnPropertyChanged();
            }
        }


        public string Meaning {
            get {
                return mMeaning;
            }
            set {
                mMeaning = value;
                OnPropertyChanged();
            }
        }
        [XmlText]
        public string Explains {
            get {
                return mExplains;
            }
            set {
                mExplains = value;
                OnPropertyChanged();
            }
        }
        public Guid KeyGuid {
            get;
            set;
        } = Guid.NewGuid();
        public int CompareTo (object obj) {
            var wi = obj as UIWord;
            return String.CompareOrdinal(this.Name, wi.Name);
        }
    }
    public class UIModule : NotificationObject
    {
        private string mName;
        public string Name {
            get {
                return mName;
            }
            set {
                mName = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UIWord> Words {
            get;
        } = new ObservableCollection<UIWord>();
        
    }
    public class UIBook : NotificationObject
    {
        private string mName;
        public string Name {
            get {
                return mName;
            }
            set {
                mName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UIModule> Modules {
            get;
        } = new ObservableCollection<UIModule>(); 
    }
}
