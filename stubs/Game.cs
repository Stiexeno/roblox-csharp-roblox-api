using RobloxCSharp.RobloxApi;

/// <summary>
/// Static accessor for Roblox services. <see cref="GetService{T}"/> lowers
/// to <c>game:GetService("T")</c>, the canonical Roblox idiom for fetching
/// a service singleton.
/// </summary>
[RobloxBuiltin]
public static class Game
{
    /// <summary>Equivalent to <c>game:GetService("T")</c>.</summary>
    public static T GetService<T>() where T : Instance => default;
}
