using System;
using System.ComponentModel;
using System.Windows;

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
            get
            {
                if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)
                {
                    return null;
                }

                return source;
            }
            set { source = value; }
        }
    }
}
