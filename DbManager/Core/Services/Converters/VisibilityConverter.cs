using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DbManager.Core.Services.Converters
{
    /// <summary>
    /// Преобразует значение(value) в параметр состояние видимости.
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        #region IValueConverter Implementation
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        } 
        #endregion
    }
}
