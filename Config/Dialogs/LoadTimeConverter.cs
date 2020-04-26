// ***********************************************************************
// Author           : the_hunter
// Created          : 04-01-2020
//
// Last Modified By : the_hunter
// Last Modified On : 04-01-2020
// ***********************************************************************

using System;
using System.Globalization;
using System.Windows.Data;

namespace Config.Dialogs
{
    /// <summary>
    /// </summary>
    internal class LoadTimeConverter : IValueConverter
    {
        /// <summary>
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return (int)value.ToString().ToMetaPluginLoadTime();
        }

        /// <summary>
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return ((MetaPluginLoadTime)value).ToCppCode();
        }
    }
}