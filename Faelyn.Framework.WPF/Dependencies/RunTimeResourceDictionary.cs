using System;
using System.ComponentModel;
using System.Windows;
using Faelyn.Framework.WPF.Helpers;

namespace Faelyn.Framework.WPF.Dependencies
{
    /// <summary>
    /// Ressource dictionary that works with design time
    /// </summary>
    public class RunTimeResourceDictionary : ResourceDictionary
    {
        private Uri source;

        public new Uri Source
        {
            get => DependencyObjectHelper.IsInDesignMode ? null : source;
            set => source = value;
        }
    }
}
