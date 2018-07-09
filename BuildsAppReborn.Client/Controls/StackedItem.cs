using System;
using System.Windows;
using System.Windows.Media;

namespace BuildsAppReborn.Client.Controls
{
    public class StackedItem
    {
        public Brush Color { get; set; }

        public String Icon { get; set; }

        public String Title { get; set; }

        public Int32 Value { get; set; }

        public Visibility Visibility => Value > 0 ? Visibility.Visible : Visibility.Collapsed;
    }
}