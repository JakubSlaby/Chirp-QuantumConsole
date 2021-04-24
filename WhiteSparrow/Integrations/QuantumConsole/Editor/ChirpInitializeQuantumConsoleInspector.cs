using QFSW.QC;
using UnityEditor;
using UnityEngine;

namespace WhiteSparrow.Integrations.QC
{
	[CustomEditor(typeof(ChirpInitializeQuantumConsoleLogger))]
	public class ChirpInitializeQuantumConsoleInspector : Editor
	{
		private static class Styles
		{
			public static readonly GUIStyle DescriptionLabel;
			public static readonly GUIStyle DescriptionLabelWarning;

			static Styles()
			{
				DescriptionLabel = new GUIStyle(EditorStyles.label);
				DescriptionLabel.richText = true;
				DescriptionLabel.wordWrap = true;
				DescriptionLabel.stretchWidth = true;
				
				DescriptionLabelWarning = new GUIStyle(DescriptionLabel);
				DescriptionLabelWarning.margin = new RectOffset(0, 0, 6, 10);
			}
		}
		private GUIContent m_WarningContent;
		private GUIContent m_MissingConsoleInstanceContent;
		private GUIContent m_ConsoleNotConvertedContent;
		private GUIContent m_ConsoleNotConfiguredContent;
		
		private void OnEnable()
		{
			m_WarningContent = EditorGUIUtility.TrTextContentWithIcon("When using Quantum Console with Chirp, few additional settings need to be configured.", MessageType.Warning);
			m_MissingConsoleInstanceContent = EditorGUIUtility.TrTextContentWithIcon("Unable to locate a Quantum Console instance in the current scene.\nIf you're instantiating the console manually, make sure it's configured to work with Chirp Logging Framework.", MessageType.Warning);
			m_ConsoleNotConvertedContent = EditorGUIUtility.TrTextContentWithIcon("Quantum Console needs to be converted to Chirp Quantum Console to work with the logging framework.", MessageType.Error);
			m_ConsoleNotConfiguredContent = EditorGUIUtility.TrTextContentWithIcon("Quantum Console is not configured correctly for Chirp Logging Framework", MessageType.Error);
		}

		private QuantumConsole FindQuantumConsoleInstance()
		{
			return GameObject.FindObjectOfType<QuantumConsole>();
		}

		public override void OnInspectorGUI()
		{
			QuantumConsole consoleInstance = FindQuantumConsoleInstance();
			if (consoleInstance == null)
			{
				GUILayout.Label(m_MissingConsoleInstanceContent, Styles.DescriptionLabelWarning);
				using (new GUILayout.HorizontalScope())
				{
					if (GUILayout.Button("Visit Github"))
					{
						
					}
					if (GUILayout.Button("Open Readme"))
					{
						
					}
				}

				return;
			}

			if (consoleInstance is ChirpQuantumConsole == false)
			{
				GUILayout.Label(m_ConsoleNotConvertedContent, Styles.DescriptionLabelWarning);
				if (GUILayout.Button("Perform Conversion"))
				{
					consoleInstance = ChirpQuantumConsoleConverter.ConvertQuantumConsole(consoleInstance);
					ChirpQuantumConsoleConfigurator.ConfigureForChirp(consoleInstance);
				}

				return;
			}

			if (consoleInstance is ChirpQuantumConsole chirpQuantumInstance)
			{
				bool isValid = ChirpQuantumConsoleConfigurator.VerifyConfiguration(chirpQuantumInstance, out string error);

				if (isValid)
				{
					GUILayout.Label("Console instance has been found and is configured correctly.");
					GUILayout.Label(error, Styles.DescriptionLabel);
				}
				else
				{
					GUILayout.Label(m_ConsoleNotConfiguredContent, Styles.DescriptionLabelWarning);
					GUILayout.Label(error, Styles.DescriptionLabel);
					if (GUILayout.Button("Configure Automatically"))
					{
						ChirpQuantumConsoleConfigurator.ConfigureForChirp(consoleInstance);
					}
				}
				
				
				
				
			}
			
		}
		
		
		
	}
}