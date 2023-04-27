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
    public unsafe partial class IConVar : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, Size = 8)]
        public partial struct __Internal
        {
            internal __IntPtr m_pConVar;

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "??0IConVar@@QEAA@AEBV0@@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr cctor(__IntPtr __instance, __IntPtr _0);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?ReplicateToPlayers@IConVar@@QEAA_NQEBHH@Z", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool ReplicateToPlayers(__IntPtr __instance, int[] pPlayers, int nPlayers);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetName@IConVar@@QEAAPEBDXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr GetName(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetDefault@IConVar@@QEAAPEBDXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr GetDefault(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetDescription@IConVar@@QEAAPEBDXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr GetDescription(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetString@IConVar@@QEAAPEBDXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr GetString(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetString@IConVar@@QEAAXPEBD@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void SetString(__IntPtr __instance, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pValue);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetFloat@IConVar@@QEAAMXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern float GetFloat(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetFloat@IConVar@@QEAAXM@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void SetFloat(__IntPtr __instance, float value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetInt@IConVar@@QEAAHXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int GetInt(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetInt@IConVar@@QEAAXH@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void SetInt(__IntPtr __instance, int value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetBool@IConVar@@QEAA_NXZ", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool GetBool(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetBool@IConVar@@QEAAX_N@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void SetBool(__IntPtr __instance, bool value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetFlags@IConVar@@QEAAHXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int GetFlags(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetFlags@IConVar@@QEAAXH@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void SetFlags(__IntPtr __instance, int flags);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetMin@IConVar@@QEAAMXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern float GetMin(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetMin@IConVar@@QEAAXM@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void SetMin(__IntPtr __instance, float value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetHasMin@IConVar@@QEAA_NXZ", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool GetHasMin(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetHasMin@IConVar@@QEAAX_N@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void SetHasMin(__IntPtr __instance, bool has);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetMax@IConVar@@QEAAMXZ", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern float GetMax(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetMax@IConVar@@QEAAXM@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void SetMax(__IntPtr __instance, float value);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?GetHasMax@IConVar@@QEAA_NXZ", CallingConvention = __CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool GetHasMax(__IntPtr __instance);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "?SetHasMax@IConVar@@QEAAX_N@Z", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void SetHasMax(__IntPtr __instance, bool has);
        }

        public __IntPtr __Instance { get; protected set; }

        internal static readonly new global::System.Collections.Concurrent.ConcurrentDictionary<IntPtr, global::SourceSharp.Core.Bridges.IConVar> NativeToManagedMap =
            new global::System.Collections.Concurrent.ConcurrentDictionary<IntPtr, global::SourceSharp.Core.Bridges.IConVar>();

        internal static void __RecordNativeToManagedMapping(IntPtr native, global::SourceSharp.Core.Bridges.IConVar managed)
        {
            NativeToManagedMap[native] = managed;
        }

        internal static bool __TryGetNativeToManagedMapping(IntPtr native, out global::SourceSharp.Core.Bridges.IConVar managed)
        {
    
            return NativeToManagedMap.TryGetValue(native, out managed);
        }

        protected bool __ownsNativeInstance;

        internal static IConVar __CreateInstance(__IntPtr native, bool skipVTables = false)
        {
            if (native == __IntPtr.Zero)
                return null;
            return new IConVar(native.ToPointer(), skipVTables);
        }

        internal static IConVar __GetOrCreateInstance(__IntPtr native, bool saveInstance = false, bool skipVTables = false)
        {
            if (native == __IntPtr.Zero)
                return null;
            if (__TryGetNativeToManagedMapping(native, out var managed))
                return (IConVar)managed;
            var result = __CreateInstance(native, skipVTables);
            if (saveInstance)
                __RecordNativeToManagedMapping(native, result);
            return result;
        }

        internal static IConVar __CreateInstance(__Internal native, bool skipVTables = false)
        {
            return new IConVar(native, skipVTables);
        }

        private static void* __CopyValue(__Internal native)
        {
            var ret = Marshal.AllocHGlobal(sizeof(__Internal));
            *(__Internal*) ret = native;
            return ret.ToPointer();
        }

        private IConVar(__Internal native, bool skipVTables = false)
            : this(__CopyValue(native), skipVTables)
        {
            __ownsNativeInstance = true;
            __RecordNativeToManagedMapping(__Instance, this);
        }

        protected IConVar(void* native, bool skipVTables = false)
        {
            if (native == null)
                return;
            __Instance = new __IntPtr(native);
        }

        public IConVar()
        {
            __Instance = Marshal.AllocHGlobal(sizeof(global::SourceSharp.Core.Bridges.IConVar.__Internal));
            __ownsNativeInstance = true;
            __RecordNativeToManagedMapping(__Instance, this);
        }

        public IConVar(global::SourceSharp.Core.Bridges.IConVar _0)
        {
            __Instance = Marshal.AllocHGlobal(sizeof(global::SourceSharp.Core.Bridges.IConVar.__Internal));
            __ownsNativeInstance = true;
            __RecordNativeToManagedMapping(__Instance, this);
            *((global::SourceSharp.Core.Bridges.IConVar.__Internal*) __Instance) = *((global::SourceSharp.Core.Bridges.IConVar.__Internal*) _0.__Instance);
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

        public bool ReplicateToPlayers(int[] pPlayers, int nPlayers)
        {
            var ___ret = __Internal.ReplicateToPlayers(__Instance, pPlayers, nPlayers);
            return ___ret;
        }

        public string Name
        {
            get
            {
                var ___ret = __Internal.GetName(__Instance);
                return CppSharp.Runtime.MarshalUtil.GetString(global::System.Text.Encoding.UTF8, ___ret);
            }
        }

        public string Default
        {
            get
            {
                var ___ret = __Internal.GetDefault(__Instance);
                return CppSharp.Runtime.MarshalUtil.GetString(global::System.Text.Encoding.UTF8, ___ret);
            }
        }

        public string Description
        {
            get
            {
                var ___ret = __Internal.GetDescription(__Instance);
                return CppSharp.Runtime.MarshalUtil.GetString(global::System.Text.Encoding.UTF8, ___ret);
            }
        }

        public string String
        {
            get
            {
                var ___ret = __Internal.GetString(__Instance);
                return CppSharp.Runtime.MarshalUtil.GetString(global::System.Text.Encoding.UTF8, ___ret);
            }

            set
            {
                __Internal.SetString(__Instance, value);
            }
        }

        public float Float
        {
            get
            {
                var ___ret = __Internal.GetFloat(__Instance);
                return ___ret;
            }

            set
            {
                __Internal.SetFloat(__Instance, value);
            }
        }

        public int Int
        {
            get
            {
                var ___ret = __Internal.GetInt(__Instance);
                return ___ret;
            }

            set
            {
                __Internal.SetInt(__Instance, value);
            }
        }

        public bool Bool
        {
            get
            {
                var ___ret = __Internal.GetBool(__Instance);
                return ___ret;
            }

            set
            {
                __Internal.SetBool(__Instance, value);
            }
        }

        public int Flags
        {
            get
            {
                var ___ret = __Internal.GetFlags(__Instance);
                return ___ret;
            }

            set
            {
                __Internal.SetFlags(__Instance, value);
            }
        }

        public float Min
        {
            get
            {
                var ___ret = __Internal.GetMin(__Instance);
                return ___ret;
            }

            set
            {
                __Internal.SetMin(__Instance, value);
            }
        }

        public bool HasMin
        {
            get
            {
                var ___ret = __Internal.GetHasMin(__Instance);
                return ___ret;
            }

            set
            {
                __Internal.SetHasMin(__Instance, value);
            }
        }

        public float Max
        {
            get
            {
                var ___ret = __Internal.GetMax(__Instance);
                return ___ret;
            }

            set
            {
                __Internal.SetMax(__Instance, value);
            }
        }

        public bool HasMax
        {
            get
            {
                var ___ret = __Internal.GetHasMax(__Instance);
                return ___ret;
            }

            set
            {
                __Internal.SetHasMax(__Instance, value);
            }
        }
    }

    public unsafe partial class ConVar
    {
        public partial struct __Internal
        {
            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "CreateConVar", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr CreateConVar([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pName, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pDefValue, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pDescription, int nFlags, bool bHasMin, float flMin, bool bHasMax, float flMax);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "FindConVar", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr FindConVar([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pName);
        }

        public static global::SourceSharp.Core.Bridges.IConVar CreateConVar(string pName, string pDefValue, string pDescription, int nFlags, bool bHasMin, float flMin, bool bHasMax, float flMax)
        {
            var ___ret = __Internal.CreateConVar(pName, pDefValue, pDescription, nFlags, bHasMin, flMin, bHasMax, flMax);
            var __result0 = global::SourceSharp.Core.Bridges.IConVar.__GetOrCreateInstance(___ret, false);
            return __result0;
        }

        public static global::SourceSharp.Core.Bridges.IConVar FindConVar(string pName)
        {
            var ___ret = __Internal.FindConVar(pName);
            var __result0 = global::SourceSharp.Core.Bridges.IConVar.__GetOrCreateInstance(___ret, false);
            return __result0;
        }
    }
}
