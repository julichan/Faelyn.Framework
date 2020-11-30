using System;
using System.Windows;
using System.Windows.Controls;
using Faelyn.Framework.Helpers;

namespace Faelyn.Framework.WPF.Helpers
{
    /// <summary>
    /// Helper to handle columns in list views
    /// </summary>
    public static class ListViewColumnsHelper
    {
        #region Constants

        /// <summary>
        /// The auto fill width property
        /// </summary>
        public static readonly DependencyProperty AutoFillWidthProperty = DependencyProperty.RegisterAttached("AutoFillWidth",
            typeof(bool),
            typeof(ListViewColumnsHelper),
            new FrameworkPropertyMetadata(false, OnAutoFillWidthChanged));
        
        /// <summary>
        /// The auto fill width property
        /// </summary>
        private static readonly DependencyProperty AutoFillWidthMultiplierProperty = DependencyProperty.RegisterAttached("AutoFillWidthMultiplier",
            typeof(double),
            typeof(ListViewColumnsHelper),
            new FrameworkPropertyMetadata(1.0d));

        /// <summary>
        /// The original width property
        /// </summary>
        private static readonly DependencyProperty OriginalWidthProperty = DependencyProperty.RegisterAttached("OriginalWidth",
            typeof(double),
            typeof(ListViewColumnsHelper),
            new FrameworkPropertyMetadata(double.NaN));

        #endregion Constants

        #region Methods

        /// <summary>
        /// Gets the auto fill property from the dependency object
        /// </summary>
        private static bool GetAutoFillWidth(DependencyObject dp)
        {
            return (bool)dp.GetValue(AutoFillWidthProperty);
        }
        
        /// <summary>
        /// Gets the auto fill property from the dependency object
        /// </summary>
        public static void SetAutoFillWidth(DependencyObject dp, bool enabled)
        {
            dp.SetValue(AutoFillWidthProperty, enabled);
        }
        
        /// <summary>
        /// Gets the auto fill property from the dependency object
        /// </summary>
        private static double GetAutoFillWidthMultiplier(DependencyObject dp)
        {
            return (double)dp.GetValue(AutoFillWidthMultiplierProperty);
        }
        
        /// <summary>
        /// Gets the auto fill property from the dependency object
        /// </summary>
        public static void SetAutoFillWidthMultiplier(DependencyObject dp, double value)
        {
            dp.SetValue(AutoFillWidthMultiplierProperty, value);
        }

        /// <summary>
        /// Gets the original width property from the dependency object
        /// </summary>
        private static double GetOriginalWidth(DependencyObject dp)
        {
            return (double)dp.GetValue(OriginalWidthProperty);
        }
        
        /// <summary>
        /// Sets the original width property from the dependency object
        /// </summary>
        private static void SetOriginalWidth(DependencyObject dp, double value)
        {
            dp.SetValue(OriginalWidthProperty, value);
        }
        

        private static void ChangeAutoFillWidth(ListView listView, bool isActive)
        {
            if (listView != null)
            {
                var gridView = listView?.View as GridView;
                if (isActive)
                {
                    ChangeColumns(gridView, true);
                    listView.SizeChanged += ListSizeChanged;
                }
                else
                {
                    listView.SizeChanged -= ListSizeChanged;
                    ChangeColumns(gridView, false);
                }
            }
        }

        private static void ChangeColumns(GridView gridView, bool isActive)
        {
            if (gridView != null)
            {
                for (int i = gridView.Columns.Count - 1; i >= 0; --i)
                {
                    var column = gridView.Columns[i];

                    if (isActive)
                    {
                        column.SetValue(OriginalWidthProperty, column.Width);
                    }
                    else
                    {
                        column.Width = GetOriginalWidth(column);
                    }
                }
            }
        }

        private static void OnAutoFillWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ChangeAutoFillWidth(sender as ListView, (bool)e.NewValue);
            }
        }

        private static void ListSizeChanged(object sender, RoutedEventArgs e)
        {
            if (sender is ListView listView && listView?.View is GridView gridView)
            {
                var actualListWidth = VisualHelper.FindParent<FrameworkElement>(listView).ActualWidth;
                var availableWidth = actualListWidth - SystemParameters.VerticalScrollBarWidth - 8;
                
                if (actualListWidth <= 0 || availableWidth <= 0) return;
                
                var nbAutoWidthElements = 0;
                var nbParts = 0.0d;
                var usedWidth = 0.0d;
                foreach(var column in gridView.Columns)
                {
                    if (GetAutoFillWidth(column))
                    {
                        nbAutoWidthElements++;
                        nbParts += GetAutoFillWidthMultiplier(column);
                    }
                    else if (double.IsNaN(column.Width))
                    {
                        SetAutoFillWidth(column, true);
                        nbAutoWidthElements++;
                        nbParts += GetAutoFillWidthMultiplier(column);
                    }
                    else
                    {
                        var width = GetOriginalWidth(column);
                        if (double.IsNaN(width))
                        {
                            width = column.Width;
                            SetOriginalWidth(column, width);
                        }
                        usedWidth += width;
                    }
                }

                if (nbAutoWidthElements == 0)
                {
                    gridView.Columns.ForEach(column => 
                        column.Width = GetOriginalWidth(column) * usedWidth / availableWidth);
                }
                else
                {
                    var remainingWidth = availableWidth - usedWidth;
                    if (remainingWidth <= 0) return;
                    var remainingWidthPerPart = remainingWidth / nbParts;
                    gridView.Columns.ForEach(column => column.Width = GetAutoFillWidth(column) 
                        ? remainingWidthPerPart * GetAutoFillWidthMultiplier(column) : GetOriginalWidth(column));
                }
            }
        }

        #endregion Methods
    }
    
}
