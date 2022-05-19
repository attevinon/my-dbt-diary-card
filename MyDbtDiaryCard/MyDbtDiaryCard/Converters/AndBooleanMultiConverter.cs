using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyDbtDiaryCard.Converters
{
    internal class AndBooleanMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = true;

            foreach (var value in values)
            {
                if (value is bool boolean)
                {
                    if (!boolean)
                        return false;
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
