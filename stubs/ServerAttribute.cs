using System;
using RobloxCSharp.RobloxApi;

/// <summary>
/// Marks a class as a server-side script. The transpiler emits the file as
/// <c>*.server.luau</c> so Roblox runs it on the server.
/// </summary>
[RobloxBuiltin]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ServerAttribute : Attribute { }
