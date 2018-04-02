using System;
using System.Windows.Input;
using System.Windows.Media;

namespace BuildsAppReborn.Client.Controls
{
    public class StackedItem
    {
        #region Public Properties

        public ICommand ClickCommand { get; set; }

        public Brush Color { get; set; }

        public String Title { get; set; }

        public Int32 Value { get; set; }

        #endregion
    }
}