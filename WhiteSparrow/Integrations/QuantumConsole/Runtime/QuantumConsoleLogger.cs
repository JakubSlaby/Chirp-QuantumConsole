﻿#if CHIRP
using QFSW.QC;
using WhiteSparrow.Integrations.QC.Logging;
using WhiteSparrow.Shared.Logging;

namespace WhiteSparrow.Integrations.QC
{
	public class QuantumConsoleLogger : AbstractLogger
	{
		public override void Initialise()
		{
		}

		public override void Destroy()
		{
		}

		public override void Append(LogEvent logEvent)
		{
			if (QuantumConsole.Instance == null)
				return;
			
			DetailedLog detailedLog = new DetailedLog(CreateString(logEvent), true)
			{
				Type = UnityLogUtil.ToUnityLogType(logEvent.level),
				Channel = logEvent.channel,
				StackTrace = logEvent.stackTrace
			};
			QuantumConsole.Instance.LogToConsole(detailedLog);
		}
	}
}
#endif