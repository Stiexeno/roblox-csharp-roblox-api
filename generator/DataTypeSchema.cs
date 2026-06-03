

namespace RobloxCSharp.RobloxApi.Generator
{
	public sealed class DataTypeSchema
	{
		public string name { get; set; } = "";
		public string deprecation_message { get; set; } = "";
		public List<string> tags { get; set; } = new();
		public List<DtConstructor> constructors { get; set; } = new();
		public List<DtProperty> properties { get; set; } = new();
		public List<DtMethod> methods { get; set; } = new();
		public List<DtMethod> functions { get; set; } = new();
		public List<DtMathOp> math_operations { get; set; } = new();
	}

	public sealed class DtConstructor
	{
		public string name { get; set; } = "";
		public List<DtParam> parameters { get; set; } = new();
		public List<string> tags { get; set; } = new();
		public string deprecation_message { get; set; } = "";
	}

	public sealed class DtProperty
	{
		public string name { get; set; } = "";
		public string type { get; set; } = "";
		public List<string> tags { get; set; } = new();
		public string deprecation_message { get; set; } = "";
	}

	public sealed class DtMethod
	{
		public string name { get; set; } = "";
		public List<DtParam> parameters { get; set; } = new();
		public List<DtReturn> returns { get; set; } = new();
		public List<string> tags { get; set; } = new();
		public string deprecation_message { get; set; } = "";
	}

	public sealed class DtParam
	{
		public string name { get; set; } = "";
		public string type { get; set; } = "";
		public string? @default { get; set; }
	}

	public sealed class DtReturn
	{
		public string type { get; set; } = "";
	}

	public sealed class DtMathOp
	{
		public string operation { get; set; } = "";
		public string type_a { get; set; } = "";
		public string type_b { get; set; } = "";
		public string return_type { get; set; } = "";
		public List<string> tags { get; set; } = new();
		public string deprecation_message { get; set; } = "";
	}
}
