using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
namespace WPF.Common
{
    public class BeginClearTextBoxAction : TriggerAction<FrameworkElement>
    {

        public static readonly DependencyProperty TextBoxProperty = DependencyProperty.Register("TextBox", typeof(TextBox), typeof(BeginClearTextBoxAction));

        public TextBox TextBox {
            get {
                return (TextBox)GetValue(TextBoxProperty);
            }
            set {
                SetValue(TextBoxProperty, value);
            }
        }
        protected override void Invoke (object parameter) {
            TextBox?.Clear();
        }
    }
}