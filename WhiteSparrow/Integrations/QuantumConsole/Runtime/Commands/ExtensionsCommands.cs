using System.Collections.Generic;
using QFSW.QC;

namespace WhiteSparrow.Integrations.QC.Commands
{
	public class ExtensionsCommands
	{
		[Command("back")]
		private static IEnumerator<ICommandAction> ExtensionsBack()
		{
			yield return new BackCommandAction();
		}

		private class BackCommandAction : AbstractChirpCommandAction
		{
			protected override void StartOnChirp(ActionContext context, ChirpQuantumConsole chirpConsole)
			{
				chirpConsole.Back();
			}
		}
	}
}