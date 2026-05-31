# roblox-csharp-roblox-api

Roblox engine API type stubs for [roblox-csharp](https://github.com/Stiexeno/roblox-csharp). Provides `Instance`, `Vector3`, every service, every enum.

The actual generation lives in `roblox-api-generator`, a CLI tool that ships in the same Aftman release zip as `roblox-csharp`. Installing the compiler installs the generator too — this plugin is just where the output lands.

## Install

```sh
roblox-csharp plugin add Stiexeno/roblox-csharp-roblox-api
```

`roblox-csharp init` does this automatically. After cloning, the CLI runs `roblox-api-generator` against `stubs/`, populating `stubs/Generated/` with the current API surface. Nothing in the repo needs to change between Roblox API releases — `git pull` followed by `roblox-csharp` re-runs the generator with whatever dump the CLI's `roblox-api-generator` knows about.

## Updating to the latest Roblox API

```sh
git -C plugins/RobloxApi pull
roblox-csharp plugin add Stiexeno/roblox-csharp-roblox-api --as RobloxApi  # re-runs the generator
# or just: roblox-api-generator plugins/RobloxApi/stubs/
```

## Repo layout

| Path | What it is |
|---|---|
| `manifest.json` | Plugin metadata. |
| `stubs/RobloxBuiltinAttribute.cs` | Marker the transpiler reads to skip emitting `require()` for engine types. |
| `stubs/Debug.cs` | Unity-style `Debug.Log` / `LogWarning` / `LogError`. Macro-lowered by the transpiler. |
| `stubs/ServerAttribute.cs`, `stubs/ClientAttribute.cs` | `[Server]` / `[Client]` markers that swap a class's output extension to `.server.luau` / `.client.luau`. |
| `stubs/GlobalUsings.cs` | `global using RobloxCSharp.RobloxApi;` so user code can reference `Instance`, `Vector3`, etc. unqualified. |
| `stubs/Generated/` | Generator output — `Classes/`, `DataTypes/`, `Enums/`. Gitignored; reproduced on install. |

The generator source lives in the main repo at [`RobloxCSharp.RobloxApiGenerator/`](https://github.com/Stiexeno/roblox-csharp/tree/main/RobloxCSharp.RobloxApiGenerator).

## License

MIT.
