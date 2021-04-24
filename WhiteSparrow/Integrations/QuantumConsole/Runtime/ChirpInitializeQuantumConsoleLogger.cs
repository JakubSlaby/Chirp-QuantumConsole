﻿using WhiteSparrow.Shared.Logging.Initialize;

namespace WhiteSparrow.Integrations.QC
{
	[ShowInitializeOptions]
	public class ChirpInitializeQuantumConsoleLogger : AbstractLoggerInitializeComponent<QuantumConsoleLogger>
	{
		public override QuantumConsoleLogger GetInstance()
		{
#if CHIRP
			return new QuantumConsoleLogger();
#else
			return null;
#endif
		}
	}
}