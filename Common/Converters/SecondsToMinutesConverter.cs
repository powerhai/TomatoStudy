using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
namespace Common.Converters {
    public class SecondsToMinutesConverter : IValueConverter {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
            var seconds = (int)value;
            var minutes = seconds / 60;
            var vsec = seconds % 60;
            if(minutes == 0)
                return $"{seconds}秒";
            else {
                return $"{minutes}分{vsec}秒";
            }
        }
        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
