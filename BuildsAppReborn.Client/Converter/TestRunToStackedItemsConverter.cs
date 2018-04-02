using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BuildsAppReborn.Client.Controls;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Client.Converter
{
    public class TestRunToStackedItemsConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var testRun = value as ITestRun;
            if (testRun == null)
            {
                return Binding.DoNothing;
            }

            var stackedItems = new List<StackedItem>();

            stackedItems.Add(new StackedItem {Value = testRun.PassedTests, Title = "Passed Tests", Color = Brushes.Green});
            stackedItems.Add(new StackedItem {Value = testRun.FailedTests, Title = "Failed Tests", Color = Brushes.Red});
            stackedItems.Add(new StackedItem {Value = testRun.UnanalyzedTests, Title = "Not run Tests", Color = Brushes.Blue});
            stackedItems.Add(new StackedItem {Value = testRun.InconclusiveTests, Title = "Inconclusive Tests", Color = Brushes.Yellow});

            return new StackedItemCollection(stackedItems) {TotalCount = testRun.TotalTests};
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}