using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace BuildsAppReborn.Contracts.UI
{
    public class DeselectionPreventionBehavior : Behavior<Selector>
    {
        #region Overrides of Base

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectionChanged += SelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.SelectionChanged -= SelectionChanged;
        }

        #endregion

        #region Private Methods

        private void SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            // when last item is deselected, apply the deselected item again
            if (e.AddedItems.Count == 0 && e.RemovedItems.Count > 0)
            {
                AssociatedObject.SelectedItem = e.RemovedItems[0];
            }
        }

        #endregion
    }
}