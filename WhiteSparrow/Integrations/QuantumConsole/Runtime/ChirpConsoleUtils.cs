using System.Text.RegularExpressions;
using QFSW.QC.Utilities;
using UnityEngine;

namespace WhiteSparrow.Integrations.QC
{
	public static class ChirpConsoleUtils
	{
		private static Regex s_HtmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
		
		public static string StripTags(string input)
		{
			return s_HtmlRegex.Replace(input, string.Empty);
		}

		public static string WrapTextSize(string text, int size)
		{
			return $"<size=\"15\">{text}</size>";
		}

		public static string WrapTextMark(string text, Color color)
		{
			color.a = 0.2f;
			if (string.IsNullOrWhiteSpace(text)) { return text; }
			string hexColor = ColorExtensions.Color32ToStringNonAlloc(color);
			return $"<mark=#{hexColor}>{text}</mark>";
		}
		
		public static string WrapTextColorByLevel(string text, LogType level)
		{
			var theme = QFSW.QC.QuantumConsole.Instance.Theme;
			if (level == LogType.Warning)
				return ColorExtensions.ColorText(text, theme.WarningColor);
		
			if (level == LogType.Assert || level == LogType.Error || level == LogType.Exception)
				return ColorExtensions.ColorText(text, theme.ErrorColor);
			
			return text;
		}
	}
}