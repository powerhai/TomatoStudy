using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;
namespace WPF.Common
{
    public class BeginStoryboardAction : TriggerAction<FrameworkElement>
    {
        public Storyboard Storyboard {
            get {
                return (Storyboard)GetValue(StoryboardProperty);
            }
            set {
                SetValue(StoryboardProperty, value);
            }
        }
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard",
            typeof(Storyboard), typeof(BeginStoryboardAction));
        protected override void Invoke (object parameter) {
            Storyboard?.Begin();
        }
    }
}
