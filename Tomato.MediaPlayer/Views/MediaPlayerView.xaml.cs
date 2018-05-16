using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Tomato.MediaPlayer.ViewModels;
namespace Tomato.MediaPlayer.Views
{
    /// <summary>
    /// Interaction logic for MediaPlayerView.xaml
    /// </summary>
    public partial class MediaPlayerView 
    { 
        public MediaPlayerView()
        {
            InitializeComponent();
        }
        private void Thumb_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            var silder = sender as Slider;
            var viewmodel = this.DataContext as MediaPlayerViewModel;
            viewmodel.JumpTo(silder.Value );
        }
 
    }
}
