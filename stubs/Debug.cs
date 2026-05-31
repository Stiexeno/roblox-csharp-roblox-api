using RobloxCSharp.RobloxApi;

// Unity-style debug macros. Calls to these are intercepted by the
// transpiler's DebugMacroOverride and rewritten to Luau print / warn /
// task.spawn(error, ...) with a `[file:line]` prefix. The C# bodies
// never execute — they exist only so user code typechecks.
//
// Kept in the global namespace deliberately so user code can write
// `Debug.Log(x)` without a `using`. RobloxBuiltin keeps the transpiler
// from emitting a require() for it the same way it does for Instance /
// Vector3 / etc.
[RobloxBuiltin]
public static class Debug
{
    public static void Log(object message) { }
    public static void LogWarning(object message) { }
    public static void LogError(object message) { }
}
