using System.Collections.Generic;
using QFSW.QC;
using WhiteSparrow.Integrations.QC.Formatting;

namespace WhiteSparrow.Integrations.QC.Logging
{
	public class LogExtensionContainer : ILogStorage
	{
		public class DefaultStorage : AbstractLogStorage
		{
			
		}
		
		private int m_MaxStoredLogs = -1;
		public int MaxStoredLogs
		{
			get => m_MaxStoredLogs;
			set
			{
				m_MaxStoredLogs = value;
				foreach (var storage in m_LogStorageStack)
				{
					storage.MaxStoredLogs = value;
				}
			}
		}

		private ILogStorage m_DefaultStorage;
		private Stack<ILogStorage> m_LogStorageStack;
		private ILogStorage m_ActiveStorage;

		public LogExtensionContainer(int maxStoredLogs)
		{
			m_LogStorageStack = new Stack<ILogStorage>();
			m_LogStorageStack.Push(m_DefaultStorage = new DefaultStorage(){ MaxStoredLogs = maxStoredLogs});
			m_ActiveStorage = m_DefaultStorage;
		}

		public IReadOnlyList<ILog> Logs => m_DefaultStorage.Logs;

		public void AddLog(ILog log)
		{
			foreach (var storage in m_LogStorageStack)
			{
				storage.AddLog(log);
			}
		}

		public void RemoveLog()
		{
			m_DefaultStorage.RemoveLog();
		}

		public void Clear()
		{
			foreach (var storage in m_LogStorageStack)
			{
				storage.Clear();
			}
			ClearLogOverwrites();
		}

		public string GetLogString()
		{
			return m_LogStorageStack.Peek().GetLogString();
		}

		public void ClearLogOverwrites()
		{
			while (m_LogStorageStack.Count > 1)
			{
				PopLogOverwrite();
			}
		}
		public void PushLogOverwrite(ILogStorage logStorage)
		{
			if (m_LogStorageStack.Contains(logStorage))
			{
				return;
			}

			logStorage.MaxStoredLogs = m_MaxStoredLogs;
			m_LogStorageStack.Push(logStorage);
			if (logStorage is ILogStorageExtension logOverwrite)
			{
				logOverwrite.Activate();
				logOverwrite.IngestLogs(m_DefaultStorage.Logs);
			}

			m_ActiveStorage = logStorage;
		}

		public ILogStorage PopLogOverwrite()
		{
			if (m_LogStorageStack.Count == 1)
			{
				return null;
			}
			
			var pop = m_LogStorageStack.Pop();
			m_ActiveStorage = m_LogStorageStack.Peek();
			if (pop is ILogStorageExtension logOverwrite)
			{
				logOverwrite.Deactivate();
				logOverwrite.Clear();
			}
			return pop;
		}

		public ILogStorage GetActiveLogStorage()
		{
			return m_ActiveStorage;
		}

		public ILog FindLog()
		{
			return null;
		}
	}
}