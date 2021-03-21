using QFSW.QC;

namespace WhiteSparrow.Integrations.QC.Commands
{
	public abstract class AbstractChirpCommandAction : ICommandAction
	{
		public virtual void Start(ActionContext context)
		{
			if (context.Console is ChirpQuantumConsole chirpConsole)
			{
				StartOnChirp(context, chirpConsole);
			}
			else
			{
				StartOnDefault(context);
			}
		}

		protected virtual void StartOnChirp(ActionContext context, ChirpQuantumConsole chirpConsole)
		{
			
		}

		protected virtual void StartOnDefault(ActionContext context)
		{
			context.Console.LogToConsole("Command available only in Chirp Quantum Console.");
		}
		

		public virtual void Finalize(ActionContext context) {}

		public virtual bool IsFinished => true;
		public virtual bool StartsIdle => true;
	}
}