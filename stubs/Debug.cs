using RobloxCSharp.RobloxApi;

/// <summary>
/// Unity-style logging shim. The transpiler macro-lowers each call into
/// Roblox's <c>print</c> / <c>warn</c> / <c>error</c> so there's no
/// runtime indirection.
/// </summary>
[RobloxBuiltin]
public static class Debug
{
    /// <summary>Prints to the output window (Roblox <c>print</c>).</summary>
    public static void Log(object message) { }

    /// <summary>Prints a warning to the output window (Roblox <c>warn</c>).</summary>
    public static void LogWarning(object message) { }

    /// <summary>Raises a Lua error (Roblox <c>error</c>).</summary>
    public static void LogError(object message) { }
}
