using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BuildsAppReborn.Client.Controls
{
    public partial class StackedBarGraph : UserControl
    {
        #region Constructors

        public StackedBarGraph()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        #endregion

        #region Public Properties

        public StackedItemCollection Items
        {
            get { return (StackedItemCollection) GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        #endregion

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", typeof(StackedItemCollection), typeof(StackedBarGraph), new PropertyMetadata(default(StackedItemCollection)));
    }
}