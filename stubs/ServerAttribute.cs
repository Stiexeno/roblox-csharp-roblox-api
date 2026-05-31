using System;
using RobloxCSharp.RobloxApi;

// Marks a class as a server-side script entry point. The compiler
// recognizes this attribute (inherited from a base class too — that's
// how ServerInstaller in the DependencyInjection plugin tags every
// derived installer) and changes the emitted file's extension to
// `.server.luau`, which Rojo treats as a Script that auto-runs.
//
// Global namespace so user code can write `[Server]` without a
// `using`. RobloxBuiltin keeps the transpiler's EmitImports from
// generating a require() for the attribute itself.
[RobloxBuiltin]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ServerAttribute : Attribute { }
