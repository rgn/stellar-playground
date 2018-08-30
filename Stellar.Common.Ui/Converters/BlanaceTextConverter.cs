using System;
using System.Globalization;
using System.Windows.Data;

namespace Stellar.Common.Ui.Converters
{
    public class BlanaceTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var balanceType = values[0] as string;            
            var balanceValue = values[1] as string;

            return $"{balanceType} {balanceValue}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
