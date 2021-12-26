using QFSW.QC;
using QFSW.QC.Utilities;
using UnityEngine;
using WhiteSparrow.Integrations.QC.LogDetails;
using WhiteSparrow.Integrations.QC.Logging;
using WhiteSparrow.Integrations.QC.Search;
#if CHIRP
using System.Collections.Generic;
using System.Reflection;
using WhiteSparrow.Shared.Logging;
#endif

namespace WhiteSparrow.Integrations.QC
{
	public class ChirpQuantumConsole : QFSW.QC.QuantumConsole
	{
		public const string Version = "0.2";
		
		private LogExtensionContainer m_ExtensionContainer;
		internal LogExtensionContainer ExtensionContainer => m_ExtensionContainer;

		private SearchLogExtension m_ExtensionSearch;
		private LogDetailsExtension m_ExtensionLogDetails;

		protected override ILogStorage CreateLogStorage()
		{
			m_ExtensionSearch = new SearchLogExtension();
			m_ExtensionLogDetails = new LogDetailsExtension();

			return m_ExtensionContainer = new LogExtensionContainer(MaxStoredLogs);
		}

		protected override ILog ConstructDebugLog(string condition, string stackTrace, LogType type, bool prependTimeStamp, bool appendStackTrace)
		{
			if (Theme)
			{
				switch (type)
				{
					case LogType.Warning:
					{
						condition = ColorExtensions.ColorText(condition, Theme.WarningColor);
						break;
					}
					case LogType.Error: 
					case LogType.Assert:
					case LogType.Exception:
					{
						condition = ColorExtensions.ColorText(condition, Theme.ErrorColor);
						break;
					}
				}
			}
			
			return new DetailedLog(condition, true)
			{
				Type = type,
				StackTrace = appendStackTrace ? stackTrace : null,
#if CHIRP
				Channel = "Unity"
#endif
			};
		}

		public void Search(string searchTerm)
		{
			m_ExtensionContainer.ClearLogOverwrites();
			m_ExtensionSearch.SearchTerm = searchTerm;
			m_ExtensionContainer.PushLogOverwrite(m_ExtensionSearch);
			RequireFlush();
		}

		internal ILog FindLog(int line)
		{
			var activeLogStorage = m_ExtensionContainer.GetActiveLogStorage();
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
			m_ExtensionLogDetails.FocusedLog = log;
			m_ExtensionContainer.PushLogOverwrite(m_ExtensionLogDetails);
			RequireFlush();
		}

		internal new void RequireFlush()
		{
			base.RequireFlush();
		}

// #if CHIRP
//
// 		public void FilterChannel(LogChannel channel)
// 		{
// 			m_ExtensionChannelFilter.FilterChannel = channel;
// 			m_ExtensionContainer.PushLogOverwrite(m_ExtensionChannelFilter);
// 			RequireFlush();
// 		}
// 		
// #endif		

		public void Back()
		{
			m_ExtensionContainer.PopLogOverwrite();
		}
	}
}