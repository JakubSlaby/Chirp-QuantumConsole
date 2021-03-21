using System.Collections.Generic;
using QFSW.QC;

namespace WhiteSparrow.Integrations.QC.Logging
{
	public class LogExtensionContainer : ILogStorage
	{
		
		private int _maxStoredLogs = -1;
		public int MaxStoredLogs
		{
			get => _maxStoredLogs;
			set
			{
				_maxStoredLogs = value;
				foreach (var storage in _logStorageStack)
				{
					storage.MaxStoredLogs = value;
				}
			}
		}

		private ILogStorage _defaultStorage;
		private Stack<ILogStorage> _logStorageStack;
		private ILogStorage _activeStorage;

		public LogExtensionContainer(ILogStorage defaultLogStorage)
		{
			_logStorageStack = new Stack<ILogStorage>();
			_logStorageStack.Push(_defaultStorage = defaultLogStorage);
			_activeStorage = _defaultStorage;
		}
		
		public void AddLog(ILog log)
		{
			foreach (var storage in _logStorageStack)
			{
				storage.AddLog(log);
			}
		}

		public void RemoveLog()
		{
			_defaultStorage.RemoveLog();
		}

		public void Clear()
		{
			foreach (var storage in _logStorageStack)
			{
				storage.Clear();
			}
		}

		public string GetLogString()
		{
			return _logStorageStack.Peek().GetLogString();
		}

		public IReadOnlyList<ILog> GetLogs()
		{
			return _defaultStorage.GetLogs();
		}

		public void ClearLogOverwrites()
		{
			while (_logStorageStack.Count > 1)
			{
				PopLogOverwrite();
			}
		}
		public void PushLogOverwrite(ILogStorage logStorage)
		{
			if (_logStorageStack.Contains(logStorage))
			{
				return;
			}

			logStorage.MaxStoredLogs = _maxStoredLogs;
			_logStorageStack.Push(logStorage);
			if (logStorage is ILogStorageExtension logOverwrite)
			{
				logOverwrite.Activate();
				logOverwrite.IngestLogs(_defaultStorage.GetLogs());
			}

			_activeStorage = logStorage;
		}

		public ILogStorage PopLogOverwrite()
		{
			if (_logStorageStack.Count == 1)
			{
				return null;
			}
			
			var pop = _logStorageStack.Pop();
			_activeStorage = _logStorageStack.Peek();
			if (pop is ILogStorageExtension logOverwrite)
			{
				logOverwrite.Deactivate();
				logOverwrite.Clear();
			}
			return pop;
		}

		public ILogStorage GetActiveLogStorage()
		{
			return _activeStorage;
		}

		public ILog FindLog()
		{
			return null;
		}
	}
}