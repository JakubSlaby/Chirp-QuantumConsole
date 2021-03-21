using System.Text.RegularExpressions;
using QFSW.QC;
using UnityEngine;
using WhiteSparrow.Integrations.QC.Formatting;
using WhiteSparrow.Integrations.QC.Logging;

namespace WhiteSparrow.Integrations.QC.Search
{
	public class SearchLogFormatter : DefaultLogFormatter
	{
		public string SearchTerm;

		protected override string FormatLogString(DetailedLog log, string logText)
		{
			return base.FormatLogString(log, FormatSearchHighlight(logText));
		}

		protected override string FormatLogString(ILog log, string logText)
		{
			return base.FormatLogString(log, FormatSearchHighlight(logText));
		}

		private string FormatSearchHighlight(string log)
		{
			string pattern = $"({SearchTerm})";
			return Regex.Replace(log, pattern, Evaluator, RegexOptions.IgnoreCase);
		}

		private string Evaluator(Match match)
		{
			return ChirpConsoleUtils.WrapTextMark(match.Value, Color.yellow);
		}
	}
}