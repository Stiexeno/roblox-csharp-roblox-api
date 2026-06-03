using RobloxCSharp.RobloxApi.Generator;

namespace RobloxApi.Tests;

public class XmlDocFormatterTests
{
	[Fact]
	public void Null_ReturnsNull()
	{
		Assert.Null(XmlDocFormatter.FormatSummary(null, "\t"));
	}

	[Fact]
	public void Empty_ReturnsNull()
	{
		Assert.Null(XmlDocFormatter.FormatSummary("", "\t"));
	}

	[Fact]
	public void Whitespace_ReturnsNull()
	{
		Assert.Null(XmlDocFormatter.FormatSummary("   \n\t  ", "\t"));
	}

	[Fact]
	public void PlainText_WrapsInSummary()
	{
		string? result = XmlDocFormatter.FormatSummary("Determines gravity.", "");

		Assert.Equal("/// <summary>\n/// Determines gravity.\n/// </summary>\n", result);
	}

	[Fact]
	public void HtmlTags_AreStripped()
	{
		string? result = XmlDocFormatter.FormatSummary("Returns a <code>Vector3</code>.", "");

		Assert.Contains("Returns a Vector3.", result);
		Assert.DoesNotContain("<code>", result);
		Assert.DoesNotContain("</code>", result);
	}

	[Fact]
	public void HtmlEntities_AreDecodedThenReEncoded()
	{
		string? result = XmlDocFormatter.FormatSummary("a &amp; b", "");

		Assert.Contains("a &amp; b", result);
	}

	[Fact]
	public void RawAmpersand_IsXmlEscaped()
	{
		string? result = XmlDocFormatter.FormatSummary("foo & bar", "");

		Assert.Contains("foo &amp; bar", result);
	}

	[Fact]
	public void UnpairedLeftAngle_IsXmlEscaped()
	{
		string? result = XmlDocFormatter.FormatSummary("if x < 5 only", "");

		Assert.Contains("if x &lt; 5 only", result);
	}

	[Fact]
	public void UnpairedRightAngle_IsXmlEscaped()
	{
		string? result = XmlDocFormatter.FormatSummary("x > 0 only", "");

		Assert.Contains("x &gt; 0 only", result);
	}

	[Fact]
	public void AngleBracketsThatLookLikeTags_AreStrippedAsTags()
	{
		string? result = XmlDocFormatter.FormatSummary("x <foo> y", "");

		Assert.Contains("x y", result);
		Assert.DoesNotContain("&lt;", result);
	}

	[Fact]
	public void Multiline_EachLineGetsPrefix()
	{
		string? result = XmlDocFormatter.FormatSummary("first line\nsecond line", "");

		Assert.Equal(
			"/// <summary>\n/// first line\n/// second line\n/// </summary>\n",
			result);
	}

	[Fact]
	public void CrLf_IsNormalizedToLf()
	{
		string? result = XmlDocFormatter.FormatSummary("a\r\nb\rc", "");

		Assert.Equal(
			"/// <summary>\n/// a\n/// b\n/// c\n/// </summary>\n",
			result);
	}

	[Fact]
	public void Indent_AppliedToEveryLine()
	{
		string? result = XmlDocFormatter.FormatSummary("hello", "\t\t");

		Assert.Equal(
			"\t\t/// <summary>\n\t\t/// hello\n\t\t/// </summary>\n",
			result);
	}

	[Fact]
	public void MultipleSpaces_CollapsedToSingle()
	{
		string? result = XmlDocFormatter.FormatSummary("foo     bar\t\ttab", "");

		Assert.Contains("foo bar tab", result);
	}

	[Fact]
	public void HtmlOnly_StripsToNothing_ReturnsNull()
	{
		string? result = XmlDocFormatter.FormatSummary("<br/><hr/>", "");

		Assert.Null(result);
	}
}
