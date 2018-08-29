using System;
using System.Globalization;
using System.Windows.Data;

namespace Stellar.Common.Ui.Converters
{
    public class AccountAbbreviatorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var account = value as string;

            if (!string.IsNullOrEmpty(account))
            {
                account = account.Substring(0, 5);
            }
            else
            {
                account = string.Empty;
            }

            return account;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
