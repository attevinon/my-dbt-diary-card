using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MyDbtDiaryCard.Converters
{
    internal class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
                return string.Empty;

            if (bool.Parse(value as string ?? "False"))
                return (culture == CultureInfo.GetCultureInfo("ru")) ? $"{value:ddd}, {value:D}" : $"{value:D}";

            return $"{value:ddd}, {value:d}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
