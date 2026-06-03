using System;
using RobloxCSharp.RobloxApi;

/// <summary>
/// Marks a class as a client-side script. The transpiler emits the file as
/// <c>*.client.luau</c> so Roblox runs it in every player's client.
/// </summary>
[RobloxBuiltin]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ClientAttribute : Attribute { }
