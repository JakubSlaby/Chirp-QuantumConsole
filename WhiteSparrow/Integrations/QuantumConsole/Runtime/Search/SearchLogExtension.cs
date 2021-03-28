using System.Globalization;
using QFSW.QC;
using WhiteSparrow.Integrations.QC.Formatting;
using WhiteSparrow.Integrations.QC.Logging;

namespace WhiteSparrow.Integrations.QC.Search
{
	public class SearchLogExtension : AbstractLogStorageExtension
	{
		private string m_SearchTerm;
		public string SearchTerm
		{
			get => m_SearchTerm;
			set
			{
				m_SearchTerm = value;
				m_SearchLogFormatter.SearchTerm = value;
			}
		}

		private SearchLogFormatter m_SearchLogFormatter = new SearchLogFormatter();

		protected override ILogFormatter LogFormatter => m_SearchLogFormatter;

		

		protected override bool FilterLog(ILog log)
		{
			if (log is DetailedLog detailedLog)
			{
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(detailedLog.TextRaw, m_SearchTerm,
					CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) != -1;
			}
			
			return CultureInfo.CurrentCulture.CompareInfo.IndexOf(ChirpConsoleUtils.StripTags(log.Text), 
				m_SearchTerm, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) != -1;
		}
	}
}