using RobloxCSharp.RobloxApi;

// Strongly-typed service accessor. Wraps Roblox's `game:GetService("X")`
// in a generic C# call: `Game.GetService<Players>()` lowers to
// `game:GetService("Players")` via the transpiler's GameServiceOverride.
// The C# body never runs.
//
// Usage:
//   var players = Game.GetService<Players>();
//   players.PlayerAdded += OnPlayerJoined;
//
// Kept in the global namespace deliberately so user code can write
// `Game.GetService<>` without a `using`. RobloxBuiltin suppresses the
// would-be require() the same way it does for Instance / Vector3.
[RobloxBuiltin]
public static class Game
{
    public static T GetService<T>() where T : Instance => default;
}
