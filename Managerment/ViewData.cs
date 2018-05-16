using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Common; 
using Models;
namespace Managerment
{
    public class ViewData : NotificationObject
    { 
        public ObservableCollection<UIBook> Books {
            get;
        } = new ObservableCollection<UIBook>(); 
    }
}
