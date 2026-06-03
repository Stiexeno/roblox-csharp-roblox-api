# roblox-csharp-roblox-api

Roblox engine API type stubs for [roblox-csharp](https://github.com/Stiexeno/roblox-csharp). Provides `Instance`, `Vector3`, every service, every enum.

The full API surface ships pre-generated in `stubs/Generated/` — install the plugin and you have `Workspace`, `Vector3`, every service, every enum ready to reference. No code generation runs at install time.

## Install

```sh
roblox-csharp plugin add Stiexeno/roblox-csharp-roblox-api
```

`roblox-csharp init` does this automatically.

## Updating to the latest Roblox API

```sh
git -C plugins/RobloxApi pull
```

The generated content is refreshed by re-running `roblox-api-generator` (see [`generator/`](./generator)) against `stubs/` and committing the result:

```sh
dotnet run --project generator -- stubs --refresh
```

## Repo layout

| Path | What it is |
|---|---|
| `manifest.json` | Plugin metadata. |
| `stubs/RobloxBuiltinAttribute.cs` | Marker the transpiler reads to skip emitting `require()` for engine types. |
| `stubs/Debug.cs` | Unity-style `Debug.Log` / `LogWarning` / `LogError`. Macro-lowered by the transpiler. |
| `stubs/Game.cs` | Static `Game.GetService<T>()` — lowered to `game:GetService("T")`. |
| `stubs/ServerAttribute.cs`, `stubs/ClientAttribute.cs` | `[Server]` / `[Client]` markers that swap a class's output extension to `.server.luau` / `.client.luau`. |
| `stubs/GlobalUsings.cs` | `global using RobloxCSharp.RobloxApi;` so user code can reference `Instance`, `Vector3`, etc. unqualified. |
| `stubs/Generated/` | Generator output — `Classes/`, `DataTypes/`, `Enums/`. Committed; every class/enum/member carries a `/// <summary>` block sourced from Roblox's own docs. |
| `generator/` | `roblox-api-generator` source — `.NET 9` console app that downloads the latest API dump + Roblox docs and emits `stubs/Generated/`. |

## License

MIT.
