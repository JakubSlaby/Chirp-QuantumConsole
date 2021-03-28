using System.Text.RegularExpressions;
using QFSW.QC;
using QFSW.QC.Utilities;
using UnityEngine;

namespace WhiteSparrow.Integrations.QC
{
	public static class ChirpConsoleUtils
	{
		private static Regex s_HtmlRegex = new Regex(@"<(.*?)>(.*?)</\1>", RegexOptions.Compiled | RegexOptions.Singleline);
		private static Regex s_NewLineRegex = new Regex("((\r\n)|(\r)|(\n))", RegexOptions.Compiled);

		public static int CountLineBreaks(string s, int fromIndex = 0, int toIndex = -1)
		{
			if (toIndex != -1)
			{
				s = s.Substring(fromIndex, Mathf.Min(s.Length - 1, toIndex));
				return s_NewLineRegex.Matches(s).Count;
			}
			
			return s_NewLineRegex.Matches(s, fromIndex).Count;
		}
		
		public static string StripTags(string input)
		{
			
			return s_HtmlRegex.Replace(input, ReplacementEvaluator);
		}

		private static string ReplacementEvaluator(Match match)
		{
			if(match.Groups.Count > 1)
				return match.Groups[1].Value;
			return string.Empty;
		}

		public static string WrapTextSize(string text, int size)
		{
			return $"<size=\"{size}\">{text}</size>";
		}

		public static string WrapTextMark(string text, Color color)
		{
			color.a = 0.2f;
			if (string.IsNullOrWhiteSpace(text)) { return text; }
			string hexColor = ColorExtensions.Color32ToStringNonAlloc(color);
			return $"<mark=#{hexColor}>{text}</mark>";
		}
		
		public static string WrapTextColorByLevel(string text, LogType level, QuantumTheme theme)
		{
			if (theme == null)
				return text;
			
			if (level == LogType.Warning)
				return ColorExtensions.ColorText(text, theme.WarningColor);
		
			if (level == LogType.Assert || level == LogType.Error || level == LogType.Exception)
				return ColorExtensions.ColorText(text, theme.ErrorColor);
			
			return text;
		}
	}
}