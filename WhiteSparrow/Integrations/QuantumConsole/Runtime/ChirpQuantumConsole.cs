using QFSW.QC;
using UnityEngine;
using WhiteSparrow.Integrations.QC.LogDetails;
using WhiteSparrow.Integrations.QC.Logging;
using WhiteSparrow.Integrations.QC.Search;

namespace WhiteSparrow.Integrations.QC
{
	public class ChirpQuantumConsole : QFSW.QC.QuantumConsole
	{
		private LogExtensionContainer _extensionContainer;
		internal LogExtensionContainer ExtensionContainer => _extensionContainer;

		private SearchLogExtension _extensionSearch;
		private LogDetailsExtension _extensionLogDetails;
		
		protected override ILogStorage CreateLogStorage()
		{
			_extensionSearch = new SearchLogExtension();
			_extensionLogDetails = new LogDetailsExtension();
			return _extensionContainer = new LogExtensionContainer(MaxStoredLogs);
		}

		protected override ILog ConstructDebugLog(string condition, string stackTrace, LogType type, bool prependTimeStamp, bool appendStackTrace)
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
			RequireFlush();
		}

		internal ILog FindLog(int line)
		{
			var activeLogStorage = _extensionContainer.GetActiveLogStorage();
			var logs = activeLogStorage.Logs;
			if (logs.Count == 0)
				return null;

			int accumulatedLines = 0;
			foreach (var log in logs)
			{
				int startLines = accumulatedLines;
				if (log.NewLine)
					accumulatedLines++;
				accumulatedLines += ChirpConsoleUtils.CountLineBreaks(log.Text);
				int endLines = accumulatedLines;

				if (line >= startLines && line < endLines)
					return log;
			}

			return null;
		}
		
		public void ShowLogDetails(ILog log)
		{
			_extensionLogDetails.FocusedLog = log;
			_extensionContainer.PushLogOverwrite(_extensionLogDetails);
			RequireFlush();
		}

		public void Back()
		{
			_extensionContainer.PopLogOverwrite();
		}
	}
}