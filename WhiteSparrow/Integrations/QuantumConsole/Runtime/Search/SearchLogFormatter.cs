using System;
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

		private static Regex s_NoparseRegexEscape = new Regex(@"(<noparse>[\s\S]*)(<mark[\s\S]*<\/mark>)([\s\S]*<\/noparse>)", RegexOptions.Compiled, TimeSpan.FromSeconds(0.5));

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
			log = Regex.Replace(log, pattern, Evaluator, RegexOptions.IgnoreCase);
			return s_NoparseRegexEscape.Replace(log, NoparseEvaluator);
		}


		private string Evaluator(Match match)
		{
			return ChirpConsoleUtils.WrapTextMark(match.Value, Color.yellow);
		}
		
		private string NoparseEvaluator(Match match)
		{
			
			return $"{match.Groups[1].Value}</noparse>{match.Groups[2].Value}<noparse>{match.Groups[3].Value}";
		}
	}
}