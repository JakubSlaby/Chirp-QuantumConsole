using QFSW.QC;
using UnityEngine;

namespace WhiteSparrow.Integrations.QC.Logging
{
	public struct DetailedLog : ILog
	{
		private string _textRaw;
		private string _textFormatted;


		public string Text
		{
			get => _textFormatted ?? _textRaw;
		}

		public string TextRaw
		{
			get => _textRaw;
			set
			{
				_textRaw = value;
				_textFormatted = null;
			}
		}
		
		public string TextFormatted
		{
			get => _textFormatted;
			set => _textFormatted = value;
		}
		
		public string StackTrace { get; set; }

		public LogType Type { get; set; }
		public bool NewLine { get; set; }
		
		public int RepeatCount { get; set; }

		public DetailedLog(string textRaw, bool newLine = true)
		{
			_textRaw = textRaw;
			_textFormatted = null;
			Type = LogType.Log;
			NewLine = newLine;
			RepeatCount = 0;
			StackTrace = null;
		}
	}
}