using System.Windows;
using System.Windows.Controls;

namespace Tomato.SpireTest.Views
{
    /// <summary>
    /// Interaction logic for DocView
    /// </summary>
    public partial class DocView : UserControl
    {
        public DocView()
        {
            InitializeComponent();
           
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
           hihi.LoadFromFile(@"C:\Projects\TomatoStudy\build\Debug\Books\abce.docx");
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            hihi.LoadFromFile(@"C:\Projects\TomatoStudy\build\Debug\Books\c.docx");
        }
    }
}
