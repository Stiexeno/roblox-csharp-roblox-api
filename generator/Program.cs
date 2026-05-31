using System.Text.Json;

namespace RobloxCSharp.RobloxApi.Generator
{
	// Public + public Main so the roblox-csharp CLI can reflect into the
	// in-process Roslyn-compiled assembly and invoke this entry point
	// directly. Standalone `dotnet run` still works the same way; the
	// runtime ignores the accessibility for the entry point.
	public static class Program
	{
		private const string ApiDumpUrl =
			"https://raw.githubusercontent.com/MaximumADHD/Roblox-Client-Tracker/roblox/Mini-API-Dump.json";

		public static async Task<int> Main(string[] args)
		{
			string apiRoot = ResolveApiRoot(args);
			string classesDir = Path.Combine(apiRoot, "Generated", "Classes");
			string enumsDir = Path.Combine(apiRoot, "Generated", "Enums");
			string dataTypesDir = Path.Combine(apiRoot, "Generated", "DataTypes");
			string cachePath = Path.Combine(Path.GetTempPath(), "roblox-api-dump.json");
			bool refresh = args.Contains("--refresh");

			Console.WriteLine($"API root: {apiRoot}");
			Console.WriteLine($"Cache:    {cachePath}");

			string json = await LoadDumpAsync(cachePath, refresh);
			ApiDump dump = JsonSerializer.Deserialize<ApiDump>(json, JsonOpts)
				?? throw new InvalidDataException("dump deserialized to null");

			Console.WriteLine($"Loaded {dump.Classes.Count} classes, {dump.Enums.Count} enums (version {dump.Version})");

			ResetDir(classesDir);
			ResetDir(enumsDir);
			ResetDir(dataTypesDir);

			int classes = 0;
			foreach (ApiClass cls in dump.Classes)
			{
				string content = ClassEmitter.Emit(cls);
				string path = Path.Combine(classesDir, FileNameFor(cls.Name) + ".cs");
				await File.WriteAllTextAsync(path, content);
				classes++;
			}

			int enums = 0;
			foreach (ApiEnum e in dump.Enums)
			{
				string content = EnumEmitter.Emit(e);
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
					content = DataTypeEmitter.Emit(name);
				}
				else
				{
					content = DataTypeBodyEmitter.Emit(schema, enumNames);
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

			// Default: walk up from the executable until we find a sibling RobloxCSharp.RobloxApi folder.
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
