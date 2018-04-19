using System;
using System.Windows;
using System.Windows.Media;

namespace BuildsAppReborn.Client.Controls
{
    public class StackedItem
    {
        #region Public Properties

        public Brush Color { get; set; }

        public String Title { get; set; }

        public Int32 Value { get; set; }

        public Visibility Visibility => Value > 0 ? Visibility.Visible : Visibility.Collapsed;

        public string Icon { get; set; }

        #endregion
    }
}