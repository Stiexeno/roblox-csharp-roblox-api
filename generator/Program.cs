using System.Text.Json;

namespace RobloxCSharp.RobloxApi.Generator
{
	internal static class Program
	{
		private const string ApiDumpUrl =
			"https://raw.githubusercontent.com/MaximumADHD/Roblox-Client-Tracker/roblox/Mini-API-Dump.json";

		private static async Task<int> Main(string[] args)
		{
			string apiRoot = ResolveApiRoot(args);
			string classesDir = Path.Combine(apiRoot, "Generated", "Classes");
			string enumsDir = Path.Combine(apiRoot, "Generated", "Enums");
			string dataTypesDir = Path.Combine(apiRoot, "Generated", "DataTypes");
			string cachePath = Path.Combine(Path.GetTempPath(), "roblox-api-dump.json");
			string docsCachePath = Path.Combine(Path.GetTempPath(), "roblox-api-docs.json");
			bool refresh = args.Contains("--refresh");

			Console.WriteLine($"API root: {apiRoot}");
			Console.WriteLine($"Cache:    {cachePath}");

			string json = await LoadDumpAsync(cachePath, refresh);
			ApiDump dump = JsonSerializer.Deserialize<ApiDump>(json, JsonOpts)
				?? throw new InvalidDataException("dump deserialized to null");

			Console.WriteLine($"Loaded {dump.Classes.Count} classes, {dump.Enums.Count} enums (version {dump.Version})");

			DocsSource docs = await DocsSource.LoadAsync(docsCachePath, refresh);

			ResetDir(classesDir);
			ResetDir(enumsDir);
			ResetDir(dataTypesDir);

			int classes = 0;
			foreach (ApiClass cls in dump.Classes)
			{
				string content = ClassEmitter.Emit(cls, docs);
				string path = Path.Combine(classesDir, FileNameFor(cls.Name) + ".cs");
				await File.WriteAllTextAsync(path, content);
				classes++;
			}

			int enums = 0;
			foreach (ApiEnum e in dump.Enums)
			{
				string content = EnumEmitter.Emit(e, docs);
				string path = Path.Combine(enumsDir, FileNameFor(e.Name) + ".cs");
				await File.WriteAllTextAsync(path, content);
				enums++;
			}

			HashSet<string> enumNames = new(dump.Enums.Select(e => e.Name));
			DataTypeYamlSource yamlSource = new(Path.Combine(Path.GetTempPath(), "rbx-datatype-yaml"));
			int dataTypes = 0, dataTypeBodies = 0;
			foreach (string name in DataTypeEmitter.Collect(dump))
			{
				DataTypeSchema? schema = await yamlSource.LoadAsync(name, refresh);
				string content;
				if (schema is null)
				{
					content = DataTypeEmitter.Emit(name, docs);
				}
				else
				{
					content = DataTypeBodyEmitter.Emit(schema, enumNames, docs);
					dataTypeBodies++;
				}
				string path = Path.Combine(dataTypesDir, FileNameFor(name) + ".cs");
				await File.WriteAllTextAsync(path, content);
				dataTypes++;
			}

			Console.WriteLine($"Emitted: {classes} classes, {enums} enums, {dataTypes} datatype files ({dataTypeBodies} with YAML bodies, {dataTypes - dataTypeBodies} empty stubs).");
			return 0;
		}

		private static string FileNameFor(string typeName)
			=> TypeMapper.SanitizeIdentifier(typeName).TrimStart('@');

		private static void ResetDir(string dir)
		{
			if (Directory.Exists(dir))
				foreach (string f in Directory.EnumerateFiles(dir, "*.cs")) File.Delete(f);
			Directory.CreateDirectory(dir);
		}

		private static string ResolveApiRoot(string[] args)
		{
			string? explicitPath = args.FirstOrDefault(a => !a.StartsWith("--"));
			if (explicitPath is not null) return Path.GetFullPath(explicitPath);

			string dir = AppContext.BaseDirectory;
			for (int i = 0; i < 8 && dir is not null; i++)
			{
				string candidate = Path.Combine(dir, "RobloxCSharp.RobloxApi");
				if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
				dir = Path.GetDirectoryName(dir)!;
			}

			return Path.GetFullPath("RobloxCSharp.RobloxApi");
		}

		private static async Task<string> LoadDumpAsync(string cachePath, bool refresh)
		{
			if (!refresh && File.Exists(cachePath))
			{
				Console.WriteLine("Using cached dump.");
				return await File.ReadAllTextAsync(cachePath);
			}

			Console.WriteLine($"Fetching {ApiDumpUrl}");
			using HttpClient http = new();
			string json = await http.GetStringAsync(ApiDumpUrl);
			await File.WriteAllTextAsync(cachePath, json);
			return json;
		}

		private static readonly JsonSerializerOptions JsonOpts = new()
		{
			PropertyNameCaseInsensitive = false,
		};
	}
}
