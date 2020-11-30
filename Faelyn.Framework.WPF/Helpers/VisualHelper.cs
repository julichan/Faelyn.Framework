using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Faelyn.Framework.WPF.Helpers
{
    /// <summary>
    /// Various helping function to use for UI
    /// </summary>
    public static class VisualHelper
    {
        #region Methods

        /// <summary>
        /// Find parent of given UI element
        /// </summary>
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                // get parent item
                var parent = LogicalTreeHelper.GetParent(child);
                if (parent == null)
                {
                    if (child is FrameworkElement)
                    {
                        parent = VisualTreeHelper.GetParent(child);
                    }

                    if (parent == null && child is ContentElement)
                    {
                        parent = ContentOperations.GetParent((ContentElement) child);
                    }
                }

                // check if the parent matches the type we're looking for
                if (parent is T wantedElement)
                {
                    return wantedElement;
                }

                child = parent;
            }

            return null;
        }

        /// <summary>
        /// Finds WPF visual children relative to given UI element
        /// </summary>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);
                    T children = child as T;
                    if (children != null)
                    {
                        yield return children;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Finds WPF visual children relative to given UI element
        /// </summary>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj, Func<T, bool> selector) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    T children = child as T;
                    if (children != null && selector(children))
                    {
                        yield return children;
                    }

                    foreach (T childOfChild in FindVisualChildren(child, selector))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the first descendant of specified type
        /// </summary>
        public static Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null)
            {
                return null;
            }
            if (element.GetType() == type)
            {
                return element;
            }
            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                {
                    break;
                }
            }
            return foundElement;
        }

        #endregion Methods
    }
}
