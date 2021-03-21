using QFSW.QC;
using UnityEngine;
using WhiteSparrow.Integrations.QC.Logging;
using WhiteSparrow.Integrations.QC.Search;

namespace WhiteSparrow.Integrations.QC
{
	public class ChirpQuantumConsole : QFSW.QC.QuantumConsole
	{
		private LogExtensionContainer _extensionContainer;
		internal LogExtensionContainer ExtensionContainer => _extensionContainer;

		private SearchLogExtension _extensionSearch;
		
		protected override ILogStorage CreateLogStorage()
		{
			_extensionSearch = new SearchLogExtension();
			return _extensionContainer = new LogExtensionContainer(new LogExtensionStorage(){ MaxStoredLogs = MaxStoredLogs});
		}

		protected override ILog DebugConstructLog(string condition, string stackTrace, LogType type, bool prependTimeStamp, bool appendStackTrace)
		{
			return new DetailedLog(condition, true)
			{
				Type = type,
				StackTrace = appendStackTrace ? stackTrace : null
			};
		}

		public void Search(string searchTerm)
		{
			_extensionContainer.ClearLogOverwrites();
			_extensionSearch.SearchTerm = searchTerm;
			_extensionContainer.PushLogOverwrite(_extensionSearch);
		}

		public void Back()
		{
			_extensionContainer.PopLogOverwrite();
		}
	}
}