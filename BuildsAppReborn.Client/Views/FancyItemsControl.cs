using System;
using System.Windows.Controls;

namespace BuildsAppReborn.Client.Views
{
    public class FancyItemsControl : ItemsControl
    {
        protected override Boolean IsItemItsOwnContainerOverride(Object item)
        {
            return false;
        }
    }
}