using System.Diagnostics;

namespace Faelyn.Framework.Helpers
{
    public static class MemoryHelper
    {
        [DebuggerHidden]
        public static void OverwriteString(ref string iStr)
        {
            if (iStr != null)
            {
                unsafe
                {
                    fixed (char* ptr = iStr)
                    {
                        for (int i = 0; i < iStr.Length; ++i)
                        {
                            ptr[i] = char.MaxValue;
                            ptr[i] = char.MinValue;
                        }
                    }
                }
            }
        }
        
        [DebuggerHidden]
        public static void OverwriteBytes(ref byte[] iAry)
        {
            if (iAry != null)
            {
                unsafe 
                {
                    fixed (byte* ptr = iAry)
                    {
                        for (int i = 0; i < iAry.Length; ++i)
                        {
                            ptr[i] = byte.MaxValue;
                            ptr[i] = byte.MinValue;
                        }
                    }
                }
            }
        }
    }
}