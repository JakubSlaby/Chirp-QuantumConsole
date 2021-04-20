using UnityEngine;
using WhiteSparrow.Shared.Logging;

namespace WhiteSparrow.Integrations.QC
{
	public class ChirpQuantumConsoleInitialize : MonoBehaviour
	{
		public void Awake()
		{
			Chirp.Initialize(new UnityConsoleLogger(), new QuantumConsoleLogger());	
		}
	}
}