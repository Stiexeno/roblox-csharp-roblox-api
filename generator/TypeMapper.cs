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
				"DataType" => SanitizeIdentifier(type.Name),
				"Enum" => $"Enums.{SanitizeIdentifier(type.Name)}",
				"Group" => "object",
				_ => "object",
			};
		}

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
