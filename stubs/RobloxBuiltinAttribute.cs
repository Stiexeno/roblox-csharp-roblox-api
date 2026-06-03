namespace RobloxCSharp.RobloxApi
{
    /// <summary>
    /// Marks a type as Roblox-engine-provided. The transpiler skips emitting
    /// <c>require()</c> calls for types tagged with this attribute since their
    /// implementation already exists in the Roblox runtime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public sealed class RobloxBuiltinAttribute : Attribute
    {
    }
}
