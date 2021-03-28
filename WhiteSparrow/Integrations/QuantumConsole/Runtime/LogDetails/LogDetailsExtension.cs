using System;
using System.Collections.Generic;
using System.Text;
using QFSW.QC;
using WhiteSparrow.Integrations.QC.Logging;

namespace WhiteSparrow.Integrations.QC.LogDetails
{
    public class LogDetailsExtension : ILogStorage
    {
        private StringBuilder m_StringBuilder = new StringBuilder();
        private ILog m_FocusedLog;

        public ILog FocusedLog
        {
            get => m_FocusedLog;
            set
            {
                m_FocusedLog = value;
                BuildString(m_FocusedLog);
            }
        }

        private void BuildString(ILog log)
        {
            m_StringBuilder.Clear();

            if (log == null)
                return;

            m_StringBuilder.AppendLine(log.Text);

            if (log is DetailedLog detailedLog)
            {
                m_StringBuilder.Append(Environment.NewLine);
                m_StringBuilder.AppendLine(detailedLog.StackTrace);
            }
                
        }

        public int MaxStoredLogs { get; set; }
        public IReadOnlyList<ILog> Logs => Array.Empty<ILog>();
        
        public void AddLog(ILog log)
        {
        }

        public void RemoveLog()
        {
        }

        public void Clear()
        {
            
        }

        public string GetLogString()
        {
            return m_StringBuilder.ToString();
        }
    }
}