using System;

namespace SourceSharp.Sdk.Enums;

[Flags]
public enum AdminFlags : uint
{
    None = 0,

    /// <summary>
    /// Reserved slot
    /// </summary>
    Reservation = 1 << 0,

    /// <summary>
    /// Generic admin abilities
    /// </summary>
    Generic = 1 << 1,

    /// <summary>
    /// Kick another user
    /// </summary>
    Kick = 1 << 2,

    /// <summary>
    /// Ban another user
    /// </summary>
    Ban = 1 << 3,

    /// <summary>
    /// Unban another user
    /// </summary>
    Unban = 1 << 4,

    /// <summary>
    /// Slay/kill/damage another user
    /// </summary>
    Slay = 1 << 5,

    /// <summary>
    /// Change the map
    /// </summary>
    ChangeMap = 1 << 6,

    /// <summary>
    /// Change basic ConVar
    /// </summary>
    ConVar = 1 << 7,

    /// <summary>
    /// Change configuration
    /// </summary>
    Config = 1 << 8,

    /// <summary>
    /// Special chat privileges
    /// </summary>
    Chat = 1 << 9,

    /// <summary>
    /// Special vote privileges
    /// </summary>
    Vote = 1 << 10,

    /// <summary>
    /// Set a server password
    /// </summary>
    Password = 1 << 11,

    /// <summary>
    /// Use RCON
    /// </summary>
    RCon = 1 << 12,

    /// <summary>
    /// Change sv_cheats and use its commands
    /// </summary>
    Cheats = 1 << 13,

    /// <summary>
    /// All access by default
    /// </summary>
    Root = 1 << 14,

    Custom1 = 1 << 15,
    Custom2 = 1 << 16,
    Custom3 = 1 << 17,
    Custom4 = 1 << 18,
    Custom5 = 1 << 19,
    Custom6 = 1 << 20,
}
