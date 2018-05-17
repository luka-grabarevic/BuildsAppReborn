using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BuildsAppReborn.Client.Controls
{
    public class StackedItemCollection : ObservableCollection<StackedItem>
    {
        public StackedItemCollection(List<StackedItem> list) : base(list)
        {
        }

        public String Title { get; set; }

        public Int32 TotalCount { get; set; }

        public Visibility Visibility => Items.Any(a => a.Visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
    }
}