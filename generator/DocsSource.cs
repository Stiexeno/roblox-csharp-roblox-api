using System.Text.Json;

namespace RobloxCSharp.RobloxApi.Generator
{
	public sealed class DocsSource
	{
		private const string DocsUrl =
			"https://raw.githubusercontent.com/MaximumADHD/Roblox-Client-Tracker/roblox/api-docs/mini/en-us.json";

		private readonly Dictionary<string, string> _docs;

		private DocsSource(Dictionary<string, string> docs) => _docs = docs;

		public static async Task<DocsSource> LoadAsync(string cachePath, bool refresh)
		{
			string json;
			if (!refresh && File.Exists(cachePath))
			{
				Console.WriteLine("Using cached docs.");
				json = await File.ReadAllTextAsync(cachePath);
			}
			else
			{
				Console.WriteLine($"Fetching {DocsUrl}");
				using HttpClient http = new();
				json = await http.GetStringAsync(DocsUrl);
				await File.WriteAllTextAsync(cachePath, json);
			}

			Dictionary<string, string> map = new(StringComparer.Ordinal);
			using JsonDocument doc = JsonDocument.Parse(json);
			foreach (JsonProperty entry in doc.RootElement.EnumerateObject())
			{
				if (entry.Value.ValueKind != JsonValueKind.Object) continue;
				if (!entry.Value.TryGetProperty("documentation", out JsonElement docEl)) continue;
				if (docEl.ValueKind != JsonValueKind.String) continue;
				string? text = docEl.GetString();
				if (string.IsNullOrWhiteSpace(text)) continue;
				map[entry.Name] = text!;
			}
			Console.WriteLine($"Loaded {map.Count} documentation entries.");
			return new DocsSource(map);
		}

		public string? ForClass(string className) => Lookup($"@roblox/globaltype/{className}");
		public string? ForClassMember(string className, string member) => Lookup($"@roblox/globaltype/{className}.{member}");
		public string? ForDataType(string typeName) => Lookup($"@roblox/global/{typeName}") ?? Lookup($"@roblox/globaltype/{typeName}");
		public string? ForDataTypeStatic(string typeName, string member) => Lookup($"@roblox/global/{typeName}.{member}");
		public string? ForDataTypeInstance(string typeName, string member) => Lookup($"@roblox/globaltype/{typeName}.{member}");
		public string? ForEnumItem(string enumName, string item) => Lookup($"@roblox/enum/{enumName}.{item}");

		private string? Lookup(string key) => _docs.TryGetValue(key, out string? v) ? v : null;
	}
}
