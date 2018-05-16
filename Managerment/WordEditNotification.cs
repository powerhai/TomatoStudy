using System;
using Common;
namespace Managerment
{
    public class WordEditNotification : ConfirmationBase {
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
 
        public string Explains {
            get {
                return mExplains;
            }
            set {
                mExplains = value;
                OnPropertyChanged();
            }
        }
        public Guid KeyGuid { get; internal set; }
        public string Meaning {
            get {
                return mMeaning;
            }
            set {
                mMeaning = value;
                OnPropertyChanged();
            }
        }
    }
}