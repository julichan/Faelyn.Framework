using System.ComponentModel;
using System.Windows;

namespace Faelyn.Framework.WPF.Helpers
{
    public static class DependencyObjectHelper
    {
        public static bool IsInDesignMode { get; } = (bool) DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
    }
}