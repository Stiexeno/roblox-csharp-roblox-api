using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RobloxCSharp.RobloxApi.Generator
{
	// Lazily downloads and caches the creator-docs YAML for each DataType.
	// Returns null for DataTypes that don't have a YAML file (some internal types,
	// some that aren't documented yet) — caller falls back to the empty stub.
	public sealed class DataTypeYamlSource
	{
		private const string BaseUrl =
			"https://raw.githubusercontent.com/Roblox/creator-docs/main/content/en-us/reference/engine/datatypes/";

		private readonly string _cacheDir;
		private readonly HttpClient _http = new();
		private readonly IDeserializer _deserializer;

		public DataTypeYamlSource(string cacheDir)
		{
			_cacheDir = cacheDir;
			Directory.CreateDirectory(_cacheDir);
			_deserializer = new DeserializerBuilder()
				.WithNamingConvention(NullNamingConvention.Instance)
				.IgnoreUnmatchedProperties()
				.Build();
		}

		public async Task<DataTypeSchema?> LoadAsync(string typeName, bool refresh)
		{
			string cachePath = Path.Combine(_cacheDir, $"{typeName}.yaml");

			string? yaml = null;
			if (!refresh && File.Exists(cachePath))
			{
				yaml = await File.ReadAllTextAsync(cachePath);
				if (yaml.Length == 0) return null; // sentinel: 404'd previously
			}

			if (yaml is null)
			{
				yaml = await TryFetchAsync(typeName);
				await File.WriteAllTextAsync(cachePath, yaml ?? "");
				if (yaml is null) return null;
			}

			try
			{
				return _deserializer.Deserialize<DataTypeSchema>(yaml);
			}
			catch
			{
				return null;
			}
		}

		private async Task<string?> TryFetchAsync(string typeName)
		{
			try
			{
				HttpResponseMessage resp = await _http.GetAsync(BaseUrl + typeName + ".yaml");
				if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
				resp.EnsureSuccessStatusCode();
				return await resp.Content.ReadAsStringAsync();
			}
			catch
			{
				return null;
			}
		}
	}
}
