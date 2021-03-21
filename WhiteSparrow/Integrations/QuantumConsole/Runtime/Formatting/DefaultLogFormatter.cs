using System.Text;
using QFSW.QC;
using QFSW.QC.Utilities;
using UnityEngine;
using WhiteSparrow.Integrations.QC.Logging;

namespace WhiteSparrow.Integrations.QC.Formatting
{
	public interface ILogFormatter
	{
		string Format(ILog log);
		string Format(Log log);
		string Format(DetailedLog log);
	}
	
	public class DefaultLogFormatter : ILogFormatter
	{
		private static StringBuilder _formattedLogBuilder = new StringBuilder();
		
		public string Format(ILog log)
		{
			if (log is DetailedLog detailedLog)
			{
				return Format(detailedLog);
			}

			return _Format(log);
		}

		public string Format(Log log)
		{
			return _Format(log);
		}
		
		public string Format(DetailedLog log)
		{
			var stringBuilder = _formattedLogBuilder.Clear();
			
			// metadata
			if (FormatMetadata(stringBuilder, log))
				FormatMetadataSeparator(stringBuilder, log);
			
			FormatLogText(stringBuilder, log);

			return stringBuilder.ToString();
		}
		private string _Format(ILog log)
		{
			var stringBuilder = _formattedLogBuilder.Clear();
			FormatLogText(stringBuilder, log);
			return stringBuilder.ToString();
		}

		protected virtual bool FormatMetadata(StringBuilder stringBuilder, DetailedLog log)
		{
			bool metadata = false;

			metadata |= FormatChannel(stringBuilder, log);
			metadata |= FormatRepeatCount(stringBuilder, log);

			return metadata;
		}

		protected virtual void FormatMetadataSeparator(StringBuilder stringBuilder, DetailedLog log)
		{
			stringBuilder.Append(' ');
		}


		protected virtual bool FormatChannel(StringBuilder stringBuilder, DetailedLog log)
		{
			// TODO: When channels are added
			return false;
		}

		protected virtual bool FormatRepeatCount(StringBuilder stringBuilder, DetailedLog log)
		{
			if (log.RepeatCount <= 0)
				return false;

			stringBuilder.Append('[');
			stringBuilder.Append(ColorExtensions.ColorText((log.RepeatCount + 1).ToString(), Color.cyan));
			stringBuilder.Append(']');

			return true;
		}
		
		protected virtual void FormatLogText(StringBuilder stringBuilder, DetailedLog log)
		{
			stringBuilder.Append(FormatLogString(log, log.TextRaw));
		}

		protected virtual string FormatLogString(DetailedLog log, string logText)
		{
			return ChirpConsoleUtils.WrapTextColorByLevel(logText, log.Type);
		}

		protected virtual void FormatLogText(StringBuilder stringBuilder, ILog log)
		{
			stringBuilder.Append(FormatLogString(log, log.Text));
		}
		
		protected virtual string FormatLogString(ILog log, string logText)
		{
			return logText;
		}
	}
	
}