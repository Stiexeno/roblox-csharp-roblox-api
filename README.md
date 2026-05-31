# roblox-csharp-roblox-api

Roblox engine API type stubs for [roblox-csharp](https://github.com/Stiexeno/roblox-csharp). Provides `Instance`, `Vector3`, every service, every enum — generated fresh from the official Roblox API dump on install. Distributed as a plugin so API updates flow via a normal `git pull` without forcing a CLI release.

## Install

```sh
roblox-csharp plugin add Stiexeno/roblox-csharp-roblox-api
```

`roblox-csharp init` installs this automatically; you don't run the command above unless you're adding it to an existing project. On install, the CLI compiles `generator/` in-process with Roslyn, runs it against the current Mini-API-Dump.json + creator-docs YAML, and populates `stubs/Generated/`. No NuGet feed, no auth, no separate SDK required — just whatever the CLI already brought along.

## Updating to the latest Roblox API

```sh
git -C plugins/RobloxApi pull
roblox-csharp
```

The next compile picks up the new generator (if any) and re-emits the stubs.

## Repo layout

| Path | What it is |
|---|---|
| `manifest.json` | Plugin metadata. `"generate": true` is the signal the CLI uses to run the generator on install. |
| `stubs/RobloxBuiltinAttribute.cs` | Marker the transpiler reads to skip emitting `require()` for engine types. |
| `stubs/Debug.cs` | Unity-style `Debug.Log` / `LogWarning` / `LogError`. Macro-lowered by the transpiler to Luau `print` / `warn` / `task.spawn(error, ...)`. |
| `stubs/ServerAttribute.cs`, `stubs/ClientAttribute.cs` | `[Server]` / `[Client]` markers that swap a class's output extension to `.server.luau` / `.client.luau`. |
| `stubs/Generated/` | Generator output — `Classes/`, `DataTypes/`, `Enums/`. Gitignored; reproduced on install. |
| `generator/*.cs` | Dump-to-C# emitter. Reads MaximumADHD's Mini-API-Dump.json + Roblox creator-docs YAML, writes typed C# stubs. |

## License

MIT.
