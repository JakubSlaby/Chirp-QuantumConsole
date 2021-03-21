using System.Globalization;
using QFSW.QC;
using WhiteSparrow.Integrations.QC.Formatting;
using WhiteSparrow.Integrations.QC.Logging;

namespace WhiteSparrow.Integrations.QC.Search
{
	public class SearchLogExtension : AbstractLogExtension
	{
		private string _searchTerm;
		public string SearchTerm
		{
			get => _searchTerm;
			set
			{
				_searchTerm = value;
				_searchLogFormatter.SearchTerm = value;
			}
		}

		private SearchLogFormatter _searchLogFormatter;

		protected override ILogFormatter CreateLogFormatter()
		{
			return _searchLogFormatter = new SearchLogFormatter();
		}

		protected override bool FilterLog(ILog log)
		{
			if (log is DetailedLog detailedLog)
			{
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(detailedLog.TextRaw, _searchTerm,
					CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) != -1;
			}
			
			return CultureInfo.CurrentCulture.CompareInfo.IndexOf(ChirpConsoleUtils.StripTags(log.Text), 
				_searchTerm, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) != -1;
		}
	}
}