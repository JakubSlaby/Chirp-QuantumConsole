using System.Collections.Generic;
using QFSW.QC;

namespace WhiteSparrow.Integrations.QC.Logging
{
	public interface ILogStorageExtension : ILogStorage
	{
		void Activate();
		void Deactivate();
		void IngestLogs(IEnumerable<ILog> logs);
	}
}