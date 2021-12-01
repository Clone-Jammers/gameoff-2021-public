using System;
using System.Runtime.InteropServices;

namespace Utilities
{
    public static class SegmentationFaultUtility
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void UNMANAGED_CALLBACK();
        
        public static void FuckMe()
        {
            var crash = (UNMANAGED_CALLBACK)Marshal.GetDelegateForFunctionPointer((IntPtr) 123, typeof(UNMANAGED_CALLBACK));
            crash();
        }
    }
}