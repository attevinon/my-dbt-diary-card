using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MyDbtDiaryCard.Converters
{
    internal class ScaleToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intvalue)
            {
                return intvalue + 1;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intvalue)
            {
                return intvalue - 1;
            }

            return null;
        }
    }
}
