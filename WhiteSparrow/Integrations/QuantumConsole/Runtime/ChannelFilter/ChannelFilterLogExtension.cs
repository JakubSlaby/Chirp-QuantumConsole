#if CHIRP
using QFSW.QC;
using WhiteSparrow.Integrations.QC.Logging;
using WhiteSparrow.Shared.Logging;

namespace WhiteSparrow.Integrations.QC.Search
{
	public class ChannelFilterLogExtension : AbstractLogStorageExtension
	{
		private LogChannel m_FilterChannel;

		public LogChannel FilterChannel
		{
			get => m_FilterChannel;
			set => m_FilterChannel = value;
		}

		protected override bool FilterLog(ILog log)
		{
			if (log is DetailedLog detailedLog)
			{
				return detailedLog.Channel.Equals(m_FilterChannel);
			}
			
			return false;
		}
	}
}
#endif