using System.Collections.Generic;
using QFSW.QC;
using UnityEngine;

namespace WhiteSparrow.Integrations.QC.Search
{
	public class SearchCommand
	{
		[Command("search")]
		public static IEnumerator<ICommandAction> Search(string searchTerm)
		{
			yield return new SearchCommandAction(searchTerm);
		}
	}
	
	internal class SearchCommandAction : ICommandAction
	{
		public readonly string searchTerm;
		
		public bool IsFinished => true;
		public bool StartsIdle => true;

		public SearchCommandAction(string searchTerm)
		{
			this.searchTerm = searchTerm;
		}
		
		public void Start(ActionContext context)
		{
			if (context.Console is ChirpQuantumConsole chirpConsole)
			{
				chirpConsole.Search(searchTerm);
			}
			else
			{
				context.Console.LogToConsole(ChirpConsoleUtils.WrapTextColorByLevel("Search requires ChirpQuantumConsole.", LogType.Error, context.Console.Theme));
			}
		}

		public void Finalize(ActionContext context)
		{
		}
	}
}