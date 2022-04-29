using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MyDbtDiaryCard.Converters
{
    internal class UsefulnessToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int _value)
            {
                if (_value == -1)
                    return "-";

                return _value;
            }

            return "fck";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "fckButReversed";
        }
    }
}
