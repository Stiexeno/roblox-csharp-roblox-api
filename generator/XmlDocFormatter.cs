using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace RobloxCSharp.RobloxApi.Generator
{
	public static class XmlDocFormatter
	{
		private static readonly Regex HtmlTag = new("<[^>]+>", RegexOptions.Compiled);
		private static readonly Regex MultiWhitespace = new(@"[ \t]+", RegexOptions.Compiled);

		public static string? FormatSummary(string? raw, string indent)
		{
			if (string.IsNullOrWhiteSpace(raw)) return null;

			string text = HtmlTag.Replace(raw, "");
			text = WebUtility.HtmlDecode(text);
			text = text.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
			if (text.Length == 0) return null;

			string[] lines = text.Split('\n');
			StringBuilder sb = new();
			sb.Append(indent).Append("/// <summary>\n");
			foreach (string line in lines)
			{
				string normalized = MultiWhitespace.Replace(line.Trim(), " ");
				string escaped = SecurityElementEscape(normalized);
				sb.Append(indent).Append("/// ").Append(escaped).Append('\n');
			}
			sb.Append(indent).Append("/// </summary>\n");
			return sb.ToString();
		}

		private static string SecurityElementEscape(string s) =>
			s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
	}
}
