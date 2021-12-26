#if CHIRP
using System;
using System.Collections.Generic;
using QFSW.QC;
using WhiteSparrow.Shared.Logging;

namespace WhiteSparrow.Integrations.QC.Search
{
	public class FilterSuggestor : IQcSuggestor
	{
		private static readonly Type s_TargetType = typeof(LogChannel);
		
		public IEnumerable<IQcSuggestion> GetSuggestions(SuggestionContext context, SuggestorOptions options)
		{
			if(context.TargetType != s_TargetType)
				return Array.Empty<IQcSuggestion>();

			
			string[] allChannelIds = LogChannel.GetAllChannelIds();
			if (allChannelIds.Length == 0)
				return Array.Empty<IQcSuggestion>();
			
			IQcSuggestion[] suggestions = new IQcSuggestion[allChannelIds.Length];
			for (int i = 0; i < allChannelIds.Length; i++)
				suggestions[i] = new RawSuggestion(allChannelIds[i]);
			
			return suggestions;
		}
	}
}
#endif