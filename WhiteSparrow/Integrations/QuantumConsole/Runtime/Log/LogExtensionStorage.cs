using QFSW.QC;
using WhiteSparrow.Integrations.QC.Formatting;

namespace WhiteSparrow.Integrations.QC.Logging
{
	public class LogExtensionStorage : LogStorage
	{
		private ILogFormatter _logFormatter;
		

		public LogExtensionStorage()
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			_logFormatter = CreateLogFormatter();
		}

		protected virtual ILogFormatter CreateLogFormatter()
		{
			return new DefaultLogFormatter();
		}
		

		public override void AddLog(ILog log)
		{
			if (log is DetailedLog detailedLog)
			{
				if (StackLog(detailedLog))
					return;
				detailedLog.TextFormatted = _logFormatter.Format(log);
				base.AddLog(detailedLog);
			}
			else
			{
				base.AddLog(new Log(_logFormatter.Format(log), log.Type, log.NewLine));
			}
		}


		protected virtual bool StackLog(DetailedLog log)
		{
			if (GetLogs().Count == 0)
				return false;
			var logs = GetLogs();
			if (logs[logs.Count - 1] is DetailedLog lastLog)
			{
				if (lastLog.Type != log.Type)
					return false;
				if (!lastLog.TextRaw.Equals(log.TextRaw))
					return false;
				if (lastLog.StackTrace != log.StackTrace)
					return false;
				
				RemoveLog();
				lastLog.RepeatCount++;
				lastLog.TextFormatted = _logFormatter.Format(lastLog);
				base.AddLog(lastLog);
				return true;
			}
			
			return false;
		}

		public virtual ILog FindLog(string lookupText, int lookupCharacter, int lookupLine)
		{
			
			return null;
		}
	}
}