#if CHIRP
using System.Collections.Generic;
using QFSW.QC;
using UnityEngine;
using WhiteSparrow.Shared.Logging;

namespace WhiteSparrow.Integrations.QC.Search
{
	public class FilterCommand
	{
	
		[Command("filter")]
		public static IEnumerator<ICommandAction> Filter(LogChannel logChannel)
		{
			yield return new FilterCommandAction(logChannel);
		}
	}

	internal class FilterCommandAction : ICommandAction
	{
		private static ChannelFilterLogExtension m_ExtensionChannelFilter;

		public bool IsFinished => true;
		public bool StartsIdle => true;

		private readonly LogChannel m_LogChannel;
		
		public FilterCommandAction(LogChannel logChannel)
		{
			m_LogChannel = logChannel;
		}
		
		public void Start(ActionContext context)
		{
			
			if (context.Console is ChirpQuantumConsole chirpConsole)
			{
				if(m_ExtensionChannelFilter == null)
					m_ExtensionChannelFilter = new ChannelFilterLogExtension();

				m_ExtensionChannelFilter.FilterChannel = m_LogChannel;
				chirpConsole.ExtensionContainer.PushLogOverwrite(m_ExtensionChannelFilter);
				chirpConsole.RequireFlush();
			}
			else
			{
				context.Console.LogToConsole(ChirpConsoleUtils.WrapTextColorByLevel("Filter requires ChirpQuantumConsole.", LogType.Error, context.Console.Theme));
			}

		}

		public void Finalize(ActionContext context)
		{
		}
		
	}
	
	public class FilterSerializer : BasicQcSerializer<LogChannel>
	{
		public override int Priority => 0;
		public override string SerializeFormatted(LogChannel value, QuantumTheme theme)
		{
			return value.ToString();
		}
	}
	
	public class FilterParser : BasicQcParser<LogChannel>
	{
		public override int Priority => 0;
		public override LogChannel Parse(string value)
		{
			return LogChannel.Get(value);
		}
	}

}
#endif