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
    public unsafe partial class SourceSharp
    {
        public partial struct __Internal
        {
            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "GetGamePath", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern __IntPtr GetGamePath();

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "LogError", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void LogError([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pMessage);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "LogMessage", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void LogMessage([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pMessage);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "GetMaxClients", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int GetMaxClients();

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "GetMaxHumanPlayers", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int GetMaxHumanPlayers();

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "GetEngineVersion", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern int GetEngineVersion();

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "ExecuteServerCommand", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void ExecuteServerCommand([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pCommand);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "InsertServerCommand", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void InsertServerCommand([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CppSharp.Runtime.UTF8Marshaller))] string pCommand);

            [SuppressUnmanagedCodeSecurity, DllImport("sourcesharp", EntryPoint = "ServerExecute", CallingConvention = __CallingConvention.Cdecl)]
            internal static extern void ServerExecute();
        }

        public static string GetGamePath()
        {
            var ___ret = __Internal.GetGamePath();
            return CppSharp.Runtime.MarshalUtil.GetString(global::System.Text.Encoding.UTF8, ___ret);
        }

        public static void LogError(string pMessage)
        {
            __Internal.LogError(pMessage);
        }

        public static void LogMessage(string pMessage)
        {
            __Internal.LogMessage(pMessage);
        }

        public static int GetMaxClients()
        {
            var ___ret = __Internal.GetMaxClients();
            return ___ret;
        }

        public static int GetMaxHumanPlayers()
        {
            var ___ret = __Internal.GetMaxHumanPlayers();
            return ___ret;
        }

        public static int GetEngineVersion()
        {
            var ___ret = __Internal.GetEngineVersion();
            return ___ret;
        }

        public static void ExecuteServerCommand(string pCommand)
        {
            __Internal.ExecuteServerCommand(pCommand);
        }

        public static void InsertServerCommand(string pCommand)
        {
            __Internal.InsertServerCommand(pCommand);
        }

        public static void ServerExecute()
        {
            __Internal.ServerExecute();
        }
    }
}
