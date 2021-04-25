using System.Text;
using QFSW.QC;
using UnityEditor;

namespace WhiteSparrow.Integrations.QC
{
	public static class ChirpQuantumConsoleConfigurator
	{
		[MenuItem("CONTEXT/ChirpQuantumConsole/Configure for Chirp Logger")]
		private static void ConfigureForChirp(MenuCommand command)
		{
			QuantumConsole source = command.context as QuantumConsole;
			if (source == null)
				return;

			ConfigureForChirp(source);
			
		}

		internal static void ConfigureForChirp(QuantumConsole source)
		{
			SerializedObject serializedObject = new SerializedObject(source);
			SerializedProperty interceptDebugLogger = serializedObject.FindProperty("_interceptDebugLogger");
			interceptDebugLogger.boolValue = false;
			
			SerializedProperty singleton = serializedObject.FindProperty("_singletonMode");
			singleton.boolValue = true;

			serializedObject.ApplyModifiedProperties();

			EditorUtility.DisplayDialog("Chirp Logger: Configure Quantum Console",
				"Quantum Console has been configured to work along Chirp Logging Framework.\nSingleton Mode: TRUE\nIntercept Debug Logger: FALSE",
				"Close");
		}

		private static StringBuilder s_HelperErrorStringBuilder = new StringBuilder();
		internal static bool VerifyConfiguration(QuantumConsole source, out string message)
		{
			SerializedObject serializedObject = new SerializedObject(source);
			SerializedProperty interceptDebugLogger = serializedObject.FindProperty("_interceptDebugLogger");
			SerializedProperty singleton = serializedObject.FindProperty("_singletonMode");
			
			s_HelperErrorStringBuilder.Clear();
			s_HelperErrorStringBuilder.AppendLine($"Singleton Mode: {MessageValueCheckOutput(singleton.boolValue, true)}"); s_HelperErrorStringBuilder.AppendLine($"Intercept Debug Messages: {MessageValueCheckOutput(interceptDebugLogger.boolValue, false)}");
			message = s_HelperErrorStringBuilder.ToString();
			
			return !interceptDebugLogger.boolValue && singleton.boolValue;
		}

		private static string MessageValueCheckOutput(bool value, bool expectedValue)
		{
			if (value == expectedValue)
				return $"<color=#3bb502><b>{value}</b></color>";
			
			return $"<color=#ff4f4f><b>{value}</b></color>";
		}
		
	}
}