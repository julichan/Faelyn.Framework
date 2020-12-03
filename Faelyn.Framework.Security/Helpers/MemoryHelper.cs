using System;
using System.Diagnostics;

namespace Faelyn.Framework.Security.Helpers
{
    public static class MemoryHelper
    {
        #if !DEBUG
        [DebuggerHidden]
        #endif
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
                            ptr[i] = Char.MaxValue;
                            ptr[i] = Char.MinValue;
                        }
                    }
                }
            }
        }
        
        #if !DEBUG
        [DebuggerHidden]
        #endif
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
                            ptr[i] = Byte.MaxValue;
                            ptr[i] = Byte.MinValue;
                        }
                    }
                }
            }
        }
    }
}