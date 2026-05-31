namespace RobloxCSharp.RobloxApi
{
	/// <summary>
	/// Marks a type as a Roblox built-in (provided as a global at runtime in Luau).
	/// The transpiler skips emitting `require(...)` for any type carrying this attribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
	public sealed class RobloxBuiltinAttribute : Attribute
	{
	}
}
