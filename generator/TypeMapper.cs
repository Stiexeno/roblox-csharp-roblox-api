namespace RobloxCSharp.RobloxApi.Generator
{

	public static class TypeMapper
	{
		public static string ToCSharp(ApiTypeRef? type)
		{
			if (type is null) return "object";

			return type.Category switch
			{
				"Primitive" => MapPrimitive(type.Name),
				"Class" => SanitizeIdentifier(type.Name),
				"DataType" => MapDataType(type.Name),
				"Enum" => $"Enums.{SanitizeIdentifier(type.Name)}",
				"Group" => "object",
				_ => "object",
			};
		}

		// `Instances` is Roblox's name for a Lua array of Instance
		// (return type of GetChildren / GetDescendants / GetPlayers /
		// GetTagged). Mapping it to `Instance[]` lets the transpiler's
		// existing array lowerings emit correct Luau (`#x`, `x[i+1]`,
		// `ipairs`) — otherwise consumers have no way to call `.Length`
		// or index into the result without a LINQ-style workaround.
		private static string MapDataType(string name) => name switch
		{
			"Instances" => "Instance[]",
			_ => SanitizeIdentifier(name),
		};

		// True for any DataType name aliased away by MapDataType —
		// DataTypeEmitter checks this to skip the stub file so we
		// don't ship a placeholder class shadowing the C# array.
		public static bool IsAliasedDataType(string name) =>
			name is "Instances";

		private static string MapPrimitive(string name) => name switch
		{
			"void" => "void",
			"bool" => "bool",
			"int" => "int",
			"int64" => "long",
			"float" => "float",
			"double" => "double",
			"string" => "string",
			_ => "object",
		};

		public static string SanitizeIdentifier(string name)
		{
			if (string.IsNullOrEmpty(name)) return "_";

			System.Text.StringBuilder sb = new(name.Length);
			foreach (char c in name)
			{
				if (char.IsLetterOrDigit(c) || c == '_')
					sb.Append(c);
			}

			string clean = sb.ToString();
			if (clean.Length == 0) return "_";
			if (char.IsDigit(clean[0])) clean = "_" + clean;

			return CSharpReservedWords.Contains(clean) ? "@" + clean : clean;
		}

		private static readonly HashSet<string> CSharpReservedWords = new()
		{
			"abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
			"class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
			"enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
			"foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is",
			"lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
			"params", "private", "protected", "public", "readonly", "ref", "return", "sbyte",
			"sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
			"this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe",
			"ushort", "using", "virtual", "void", "volatile", "while",
		};
	}
}
