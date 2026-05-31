using System;
using RobloxCSharp.RobloxApi;

// Marks a class as a client-side script entry point. Same mechanism as
// ServerAttribute — the compiler emits `.client.luau` instead, which
// Rojo treats as a LocalScript.
[RobloxBuiltin]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ClientAttribute : Attribute { }
