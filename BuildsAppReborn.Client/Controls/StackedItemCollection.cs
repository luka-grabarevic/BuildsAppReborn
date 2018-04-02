using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BuildsAppReborn.Client.Controls
{
    public class StackedItemCollection : ObservableCollection<StackedItem>
    {
        #region Constructors

        public StackedItemCollection(List<StackedItem> list) : base(list)
        {
        }

        #endregion

        #region Public Properties

        public Int32 TotalCount { get; set; }

        #endregion
    }
}