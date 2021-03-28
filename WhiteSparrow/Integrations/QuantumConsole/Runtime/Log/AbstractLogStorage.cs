using System.Collections.Generic;
using QFSW.QC;
using WhiteSparrow.Integrations.QC.Formatting;

namespace WhiteSparrow.Integrations.QC.Logging
{
	public abstract class AbstractLogStorage : ILogStorage
	{
		private LogStorage m_Storage = new LogStorage();
		protected virtual ILogFormatter LogFormatter => DefaultLogFormatter.Instance;

		public int MaxStoredLogs
		{
			get => m_Storage.MaxStoredLogs;
			set => m_Storage.MaxStoredLogs = value;
		}

		public IReadOnlyList<ILog> Logs => m_Storage.Logs;

		public virtual void AddLog(ILog log)
		{
			if (log is DetailedLog detailedLog)
			{
				if (StackLog(detailedLog))
					return;
				if(LogFormatter != null)
					detailedLog.TextFormatted = LogFormatter.Format(log);
				m_Storage.AddLog(detailedLog);
			}
			else
			{
				m_Storage.AddLog(new Log(LogFormatter.Format(log), log.Type, log.NewLine));
			}
		}
		
		protected virtual bool StackLog(DetailedLog log)
		{
			if (Logs.Count == 0)
				return false;
			var logs = Logs;
			if (logs[logs.Count - 1] is DetailedLog lastLog)
			{
				if (lastLog.Type != log.Type)
					return false;
				if (!lastLog.TextRaw.Equals(log.TextRaw))
					return false;
				if (lastLog.StackTrace != log.StackTrace)
					return false;
				
				m_Storage.RemoveLog();
				lastLog.RepeatCount++;
				if(LogFormatter != null)
					lastLog.TextFormatted = LogFormatter.Format(lastLog);
				m_Storage.AddLog(lastLog);
				return true;
			}
			
			return false;
		}


		public virtual void RemoveLog()
		{
			m_Storage.RemoveLog();
		}


		public void Clear()
		{
			m_Storage.Clear();
		}

		public string GetLogString()
		{
			return m_Storage.GetLogString();
		}
	}
}