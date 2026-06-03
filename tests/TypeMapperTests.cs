using RobloxCSharp.RobloxApi.Generator;

namespace RobloxApi.Tests;

public class TypeMapperTests
{
	[Fact]
	public void ToCSharp_Null_ReturnsObject()
	{
		Assert.Equal("object", TypeMapper.ToCSharp(null));
	}

	[Theory]
	[InlineData("void", "void")]
	[InlineData("bool", "bool")]
	[InlineData("int", "int")]
	[InlineData("int64", "long")]
	[InlineData("float", "float")]
	[InlineData("double", "double")]
	[InlineData("string", "string")]
	public void ToCSharp_Primitive_MapsCorrectly(string yamlName, string expected)
	{
		Assert.Equal(expected, TypeMapper.ToCSharp(new ApiTypeRef { Category = "Primitive", Name = yamlName }));
	}

	[Fact]
	public void ToCSharp_UnknownPrimitive_FallsBackToObject()
	{
		Assert.Equal("object", TypeMapper.ToCSharp(new ApiTypeRef { Category = "Primitive", Name = "decimal" }));
	}

	[Fact]
	public void ToCSharp_Class_SanitizesIdentifier()
	{
		Assert.Equal("Workspace", TypeMapper.ToCSharp(new ApiTypeRef { Category = "Class", Name = "Workspace" }));
	}

	[Fact]
	public void ToCSharp_DataType_SanitizesIdentifier()
	{
		Assert.Equal("Vector3", TypeMapper.ToCSharp(new ApiTypeRef { Category = "DataType", Name = "Vector3" }));
	}

	[Fact]
	public void ToCSharp_Enum_PrefixedWithEnumsNamespace()
	{
		Assert.Equal("Enums.Material", TypeMapper.ToCSharp(new ApiTypeRef { Category = "Enum", Name = "Material" }));
	}

	[Fact]
	public void ToCSharp_Group_FallsBackToObject()
	{
		Assert.Equal("object", TypeMapper.ToCSharp(new ApiTypeRef { Category = "Group", Name = "Tuple" }));
	}

	[Fact]
	public void ToCSharp_UnknownCategory_FallsBackToObject()
	{
		Assert.Equal("object", TypeMapper.ToCSharp(new ApiTypeRef { Category = "Mystery", Name = "Foo" }));
	}

	[Fact]
	public void SanitizeIdentifier_Empty_ReturnsUnderscore()
	{
		Assert.Equal("_", TypeMapper.SanitizeIdentifier(""));
	}

	[Fact]
	public void SanitizeIdentifier_Plain_PassesThrough()
	{
		Assert.Equal("Workspace", TypeMapper.SanitizeIdentifier("Workspace"));
	}

	[Fact]
	public void SanitizeIdentifier_WithSpaces_AreStripped()
	{
		Assert.Equal("FooBar", TypeMapper.SanitizeIdentifier("Foo Bar"));
	}

	[Fact]
	public void SanitizeIdentifier_WithSpecialChars_AreStripped()
	{
		Assert.Equal("FooBar", TypeMapper.SanitizeIdentifier("Foo-Bar!"));
	}

	[Fact]
	public void SanitizeIdentifier_LeadingDigit_GetsUnderscorePrefix()
	{
		Assert.Equal("_123Foo", TypeMapper.SanitizeIdentifier("123Foo"));
	}

	[Theory]
	[InlineData("class", "@class")]
	[InlineData("event", "@event")]
	[InlineData("operator", "@operator")]
	[InlineData("namespace", "@namespace")]
	public void SanitizeIdentifier_ReservedWord_GetsAtPrefix(string reserved, string expected)
	{
		Assert.Equal(expected, TypeMapper.SanitizeIdentifier(reserved));
	}

	[Fact]
	public void SanitizeIdentifier_AllInvalid_ReturnsUnderscore()
	{
		Assert.Equal("_", TypeMapper.SanitizeIdentifier("!!!"));
	}

	[Fact]
	public void SanitizeIdentifier_NonReservedWord_NoAtPrefix()
	{
		Assert.Equal("Player", TypeMapper.SanitizeIdentifier("Player"));
		Assert.DoesNotContain("@", TypeMapper.SanitizeIdentifier("Player"));
	}
}
