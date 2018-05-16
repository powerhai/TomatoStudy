using System.Windows.Controls;
namespace WPF.Controls.Busy
{
    /// <summary>
    /// RegionBusyBox.xaml 的交互逻辑
    /// </summary>
   // [Export(typeof(IBusy))]
    public partial class BusyBox  
    {
        public BusyBox()
        {  
            InitializeComponent();
        }
 
        private int ShowCount = 0;
        private object LockObject = new object();
 
 
    }
}
