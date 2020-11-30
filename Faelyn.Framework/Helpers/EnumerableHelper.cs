using System;
using System.Collections.Generic;

namespace Faelyn.Framework.Helpers
{
    /// <summary>
    /// Helpers based on enumerable objects
    /// </summary>
    public static class EnumerableHelper
    {
        /// <summary>
        /// Iterate through an enumerable and perform a custom action on each items
        /// </summary>
        public static IEnumerable<TOut> Each<TItem, TOut>(this IEnumerable<TItem> items, Func<TItem, TOut> function)
        {
            foreach(var item in items)
            {
                yield return function(item);
            }
        }
        
        /// <summary>
        /// Iterate through an array and perform a custom action on each items
        /// </summary>
        public static IEnumerable<TOut> Each<TItem, TOut>(this TItem[] items, Func<TItem, TOut> function)
        {
            foreach (var item in items)
            {
                yield return function(item);
            }
        }
        
        /// <summary>
        /// Iterate through an enumerable and perform a custom action on each items
        /// </summary>
        public static void ForEach<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
        {
            foreach(var item in items)
            {
                action(item);
            }
        }

        /// <summary>
        /// Iterate through an array and perform a custom action on each items
        /// </summary>
        public static void ForEach<TItem>(this TItem[] items, Action<TItem> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}