using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildsAppReborn.Client.Controls
{
    public partial class StackedBarGraph : UserControl
    {
        #region Constructors

        public StackedBarGraph()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        public ICommand ClickCommand
        {
            get { return (ICommand) GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public Object ClickCommandParameter
        {
            get { return GetValue(ClickCommandParameterProperty); }
            set { SetValue(ClickCommandParameterProperty, value); }
        }

        public StackedItemCollection Items
        {
            get { return (StackedItemCollection) GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public String Title
        {
            get { return (String) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion


        public static readonly DependencyProperty ClickCommandParameterProperty = DependencyProperty.Register(
            "ClickCommandParameter", typeof(Object), typeof(StackedBarGraph), new PropertyMetadata(default(Object)));

        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
            "ClickCommand", typeof(ICommand), typeof(StackedBarGraph), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(String), typeof(StackedBarGraph), new PropertyMetadata(default(String)));

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", typeof(StackedItemCollection), typeof(StackedBarGraph), new PropertyMetadata(default(StackedItemCollection)));
    }
}