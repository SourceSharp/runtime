// ----------------------------------------------------------------------------
// <auto-generated>
// This is autogenerated code by CppSharp.
// Do not edit this file or all your changes will be lost after re-generation.
// </auto-generated>
// ----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Security;
using __CallingConvention = global::System.Runtime.InteropServices.CallingConvention;
using __IntPtr = global::System.IntPtr;

#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required

namespace SourceSharp.Core.Bridges
{
    public unsafe partial class SSGameEvent : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, Size = 8)]
        public partial struct __Internal
        {
            internal __IntPtr m_pEvent;

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "??0SSGameEvent@@QEAA@AEBV0@@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr cctor(__IntPtr __instance, __IntPtr _0);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetInt@SSGameEvent@@QEAA_NPEBDH@Z", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool SetInt(__IntPtr __instance, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pKey, int value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetInt@SSGameEvent@@QEAAHPEBD@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int GetInt(__IntPtr __instance, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pKey);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetBool@SSGameEvent@@QEAA_NPEBD_N@Z", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool SetBool(__IntPtr __instance, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pKey, bool value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetBool@SSGameEvent@@QEAA_NPEBD@Z", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool GetBool(__IntPtr __instance, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pKey);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetFloat@SSGameEvent@@QEAA_NPEBDM@Z", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool SetFloat(__IntPtr __instance, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pKey, float value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetFloat@SSGameEvent@@QEAAMPEBD@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern float GetFloat(__IntPtr __instance, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pKey);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetString@SSGameEvent@@QEAA_NPEBD0@Z", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool SetString(__IntPtr __instance, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pKey, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetString@SSGameEvent@@QEAAPEBDPEBD@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr GetString(__IntPtr __instance, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pKey);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetBroadcast@SSGameEvent@@QEAA_N_N@Z", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool SetBroadcast(__IntPtr __instance, bool value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?Cancel@SSGameEvent@@QEAAXXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void Cancel(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetName@SSGameEvent@@QEAAPEBDXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr GetName(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetBroadcast@SSGameEvent@@QEAA_NXZ", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool GetBroadcast(__IntPtr __instance);
        }

        public __IntPtr __Instance { get; protected set; }

        internal static readonly new global::System.Collections.Concurrent.ConcurrentDictionary<IntPtr, global::SourceSharp.Core.Bridges.SSGameEvent> NativeToManagedMap =
            new global::System.Collections.Concurrent.ConcurrentDictionary<IntPtr, global::SourceSharp.Core.Bridges.SSGameEvent>();

        internal static void __RecordNativeToManagedMapping(IntPtr native, global::SourceSharp.Core.Bridges.SSGameEvent managed)
        {
            NativeToManagedMap[native] = managed;
        }

        internal static bool __TryGetNativeToManagedMapping(IntPtr native, out global::SourceSharp.Core.Bridges.SSGameEvent managed)
        {
    
            return NativeToManagedMap.TryGetValue(native, out managed);
        }

        protected bool __ownsNativeInstance;

        internal static SSGameEvent __CreateInstance(__IntPtr native, bool skipVTables = false)
        {
            if (native == __IntPtr.Zero)
                return null;
            return new SSGameEvent(native.ToPointer(), skipVTables);
        }

        internal static SSGameEvent __GetOrCreateInstance(__IntPtr native, bool saveInstance = false, bool skipVTables = false)
        {
            if (native == __IntPtr.Zero)
                return null;
            if (__TryGetNativeToManagedMapping(native, out var managed))
                return (SSGameEvent)managed;
            var result = __CreateInstance(native, skipVTables);
            if (saveInstance)
                __RecordNativeToManagedMapping(native, result);
            return result;
        }

        internal static SSGameEvent __CreateInstance(__Internal native, bool skipVTables = false)
        {
            return new SSGameEvent(native, skipVTables);
        }

        private static void* __CopyValue(__Internal native)
        {
            var ret = Marshal.AllocHGlobal(sizeof(__Internal));
            *(__Internal*) ret = native;
            return ret.ToPointer();
        }

        private SSGameEvent(__Internal native, bool skipVTables = false)
            : this(__CopyValue(native), skipVTables)
        {
            __ownsNativeInstance = true;
            __RecordNativeToManagedMapping(__Instance, this);
        }

        protected SSGameEvent(void* native, bool skipVTables = false)
        {
            if (native == null)
                return;
            __Instance = new __IntPtr(native);
        }

        public SSGameEvent()
        {
            __Instance = Marshal.AllocHGlobal(sizeof(global::SourceSharp.Core.Bridges.SSGameEvent.__Internal));
            __ownsNativeInstance = true;
            __RecordNativeToManagedMapping(__Instance, this);
        }

        public SSGameEvent(global::SourceSharp.Core.Bridges.SSGameEvent _0)
        {
            __Instance = Marshal.AllocHGlobal(sizeof(global::SourceSharp.Core.Bridges.SSGameEvent.__Internal));
            __ownsNativeInstance = true;
            __RecordNativeToManagedMapping(__Instance, this);
            *((global::SourceSharp.Core.Bridges.SSGameEvent.__Internal*) __Instance) = *((global::SourceSharp.Core.Bridges.SSGameEvent.__Internal*) _0.__Instance);
        }

        public void Dispose()
        {
            Dispose(disposing: true, callNativeDtor : __ownsNativeInstance );
        }

        partial void DisposePartial(bool disposing);

        internal protected virtual void Dispose(bool disposing, bool callNativeDtor )
        {
            if (__Instance == IntPtr.Zero)
                return;
            NativeToManagedMap.TryRemove(__Instance, out _);
            DisposePartial(disposing);
            if (__ownsNativeInstance)
                Marshal.FreeHGlobal(__Instance);
            __Instance = IntPtr.Zero;
        }

        public bool SetInt(string pKey, int value)
        {
            var ___ret = __Internal.SetInt(__Instance, pKey, value);
            return ___ret;
        }

        public int GetInt(string pKey)
        {
            var ___ret = __Internal.GetInt(__Instance, pKey);
            return ___ret;
        }

        public bool SetBool(string pKey, bool value)
        {
            var ___ret = __Internal.SetBool(__Instance, pKey, value);
            return ___ret;
        }

        public bool GetBool(string pKey)
        {
            var ___ret = __Internal.GetBool(__Instance, pKey);
            return ___ret;
        }

        public bool SetFloat(string pKey, float value)
        {
            var ___ret = __Internal.SetFloat(__Instance, pKey, value);
            return ___ret;
        }

        public float GetFloat(string pKey)
        {
            var ___ret = __Internal.GetFloat(__Instance, pKey);
            return ___ret;
        }

        public bool SetString(string pKey, string value)
        {
            var ___ret = __Internal.SetString(__Instance, pKey, value);
            return ___ret;
        }

        public string GetString(string pKey)
        {
            var ___ret = __Internal.GetString(__Instance, pKey);
            return CppSharp.Runtime.MarshalUtil.GetString(global::System.Text.Encoding.UTF8, ___ret);
        }

        public bool SetBroadcast(bool value)
        {
            var ___ret = __Internal.SetBroadcast(__Instance, value);
            return ___ret;
        }

        public void Cancel()
        {
            __Internal.Cancel(__Instance);
        }

        public string Name
        {
            get
            {
                var ___ret = __Internal.GetName(__Instance);
                return CppSharp.Runtime.MarshalUtil.GetString(global::System.Text.Encoding.UTF8, ___ret);
            }
        }

        public bool Broadcast
        {
            get
            {
                var ___ret = __Internal.GetBroadcast(__Instance);
                return ___ret;
            }

            set
            {
                __Internal.SetBroadcast(__Instance, value);
            }
        }
    }

    public unsafe partial class Event
    {
        public partial struct __Internal
        {
            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "RegGameEventHook", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool RegGameEventHook([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pName);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "CreateGameEvent", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr CreateGameEvent([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pName, bool bBroadcast);
        }

        public static bool RegGameEventHook(string pName)
        {
            var ___ret = __Internal.RegGameEventHook(pName);
            return ___ret;
        }

        public static global::SourceSharp.Core.Bridges.SSGameEvent CreateGameEvent(string pName, bool bBroadcast)
        {
            var ___ret = __Internal.CreateGameEvent(pName, bBroadcast);
            var __result0 = global::SourceSharp.Core.Bridges.SSGameEvent.__GetOrCreateInstance(___ret, false);
            return __result0;
        }
    }
}
