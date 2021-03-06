using System;
using System.Windows;
using Faelyn.Framework.WPF.Helpers;

namespace Faelyn.Framework.WPF.Dependencies
{
    /// <summary>
    /// Ressource dictionary that works with design time
    /// </summary>
    public class DesignTimeResourceDictionary : ResourceDictionary
    {
        private Uri source;

        public new Uri Source
        {
            get => DependencyObjectHelper.IsInDesignMode ? source : null;
            set => source = value;
        }
    }
}
