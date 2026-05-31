// Global usings so user code and downstream plugin stubs (Component,
// IAssetsService, etc.) can reference `Instance`, `Vector3`, services,
// and enums unqualified — without adding `using RobloxCSharp.RobloxApi;`
// at the top of every file. Lives in the plugin's stubs/ root so it's
// picked up by the user's csproj implicit Compile glob along with the
// generated type files.

global using RobloxCSharp.RobloxApi;
global using RobloxCSharp.RobloxApi.Enums;
