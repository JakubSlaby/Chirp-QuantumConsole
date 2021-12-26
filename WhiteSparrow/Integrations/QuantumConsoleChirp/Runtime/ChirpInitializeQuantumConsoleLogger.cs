#if CHIRP

using WhiteSparrow.Shared.Logging.Initialize;

namespace WhiteSparrow.Integrations.QC
{
	[ShowInitializeOptions]
	public class ChirpInitializeQuantumConsoleLogger : AbstractLoggerInitializeComponent<QuantumConsoleLogger>
	{
		public override QuantumConsoleLogger GetInstance()
		{
			return new QuantumConsoleLogger();
		}
	}
}
#endif