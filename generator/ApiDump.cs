using System.Text.Json;
using System.Text.Json.Serialization;

namespace RobloxCSharp.RobloxApi.Generator
{
	// Some Function members return multiple values (Lua-style multi-return).
	// The dump uses an array for those and a single object for the common case.
	// Normalize both shapes to a list.
	public sealed class ReturnTypeConverter : JsonConverter<List<ApiTypeRef>>
	{
		public override List<ApiTypeRef> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.StartArray)
				return JsonSerializer.Deserialize<List<ApiTypeRef>>(ref reader, options) ?? new();

			ApiTypeRef? single = JsonSerializer.Deserialize<ApiTypeRef>(ref reader, options);
			return single is null ? new() : new() { single };
		}

		public override void Write(Utf8JsonWriter writer, List<ApiTypeRef> value, JsonSerializerOptions options)
			=> throw new NotSupportedException();
	}

	// Models the relevant subset of the Roblox API dump
	// (https://github.com/MaximumADHD/Roblox-Client-Tracker/blob/roblox/Mini-API-Dump.json).
	// Unmapped fields are dropped — System.Text.Json ignores them by default.

	public sealed class ApiDump
	{
		public List<ApiClass> Classes { get; set; } = new();
		public List<ApiEnum> Enums { get; set; } = new();
		public int Version { get; set; }
	}

	public sealed class ApiClass
	{
		public string Name { get; set; } = "";
		public string Superclass { get; set; } = "";
		public List<JsonElement> Tags { get; set; } = new();
		public List<ApiMember> Members { get; set; } = new();
	}

	public sealed class ApiEnum
	{
		public string Name { get; set; } = "";
		public List<ApiEnumItem> Items { get; set; } = new();
	}

	public sealed class ApiEnumItem
	{
		public string Name { get; set; } = "";
		public int Value { get; set; }
	}

	public sealed class ApiMember
	{
		public string MemberType { get; set; } = "";
		public string Name { get; set; } = "";
		public List<JsonElement> Tags { get; set; } = new();

		// Property
		public ApiTypeRef? ValueType { get; set; }

		// Function / Event / Callback
		public List<ApiParam> Parameters { get; set; } = new();

		[JsonConverter(typeof(ReturnTypeConverter))]
		public List<ApiTypeRef> ReturnType { get; set; } = new();
	}

	public sealed class ApiTypeRef
	{
		public string Category { get; set; } = "";
		public string Name { get; set; } = "";
	}

	public sealed class ApiParam
	{
		public string Name { get; set; } = "";
		public ApiTypeRef? Type { get; set; }
	}

	public static class ApiMemberExtensions
	{
		public static bool HasTag(this ApiMember m, string tag) => HasTag(m.Tags, tag);
		public static bool HasTag(this ApiClass c, string tag) => HasTag(c.Tags, tag);

		private static bool HasTag(List<JsonElement> tags, string tag)
		{
			foreach (JsonElement t in tags)
			{
				if (t.ValueKind == JsonValueKind.String && t.GetString() == tag)
					return true;
			}
			return false;
		}
	}
}
