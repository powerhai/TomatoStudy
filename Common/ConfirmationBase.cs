using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common.Annotations;
using Prism.Interactivity.InteractionRequest;
namespace Common
{
    public class ConfirmationBase : Confirmation, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}