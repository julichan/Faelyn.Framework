using System;

namespace Faelyn.Framework.Helpers
{
    public static class WeakReferenceHelper
    {
        public static object GetTargetSafe(this WeakReference reference)
        {
            object result = null;

            // Never trust WeakReference.IsAlive
            try
            {
                if (reference.IsAlive)
                {
                    result = reference.Target;
                }
            }
            catch
            {
                // Ignore the error, the object has been garbage collected before it was retrieved.
            }

            return result;
        }

        public static TType GetTargetSafe<TType>(this WeakReference reference)
        {
            var target = reference.GetTargetSafe();

            return target is TType targetType ? targetType : default;
        }
    }
}