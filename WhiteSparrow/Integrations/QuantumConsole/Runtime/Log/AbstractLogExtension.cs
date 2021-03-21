using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using QFSW.QC;

namespace WhiteSparrow.Integrations.QC.Logging
{
	public abstract class AbstractLogExtension : LogExtensionStorage, ILogStorageExtension
	{
		
		public virtual void Activate()
		{
		}

		public virtual void Deactivate()
		{
			if(_ingestTaskCancellation != null && _ingestTaskCancellation.IsCancellationRequested)
				_ingestTaskCancellation.Cancel();
			Clear();
		}

		public override void AddLog(ILog log)
		{
			if (!FilterLog(log))
				return;

			if (IsIngestingLogs)
			{
				QueueLog(log);
				return;
			}

			base.AddLog(log);
		}

		private void _AddLog(ILog log)
		{
			if (!FilterLog(log))
				return;
			base.AddLog(log);
		}

		protected virtual bool FilterLog(ILog log)
		{
			return true;
		}

		private ConcurrentQueue<ILog> _ingestQueue = new ConcurrentQueue<ILog>();
		protected virtual void QueueLog(ILog log)
		{
			_ingestQueue.Enqueue(log);
		
			while (MaxStoredLogs > 0 && _ingestQueue.Count > MaxStoredLogs)
				_ingestQueue.TryDequeue(out _);
			
		}

		#region Log Ingestion

		
		private Task _ingestTask;
		private CancellationTokenSource _ingestTaskCancellation;

		public virtual void IngestLogs(IEnumerable<ILog> logs)
		{
			if (_ingestTask != null && !_ingestTask.IsCompleted)
				return;

			foreach (var log in logs)
				QueueLog(log);
			
			_ingestTaskCancellation = new CancellationTokenSource();
			_ingestTask = IngestProcessing(_ingestTaskCancellation.Token);
		}

		public bool IsIngestingLogs => _ingestTask != null && !_ingestTask.IsCompleted;

#pragma warning disable 1998
		private async Task IngestProcessing(CancellationToken cancellationToken)
#pragma warning restore 1998
		{
			while (!_ingestQueue.IsEmpty)
			{
				if (_ingestQueue.TryDequeue(out var log))
					_AddLog(log);
			}
		}
		
		#endregion

	}
}