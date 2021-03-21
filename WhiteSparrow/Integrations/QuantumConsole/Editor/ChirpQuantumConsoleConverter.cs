using System;
using System.Collections.Generic;
using QFSW.QC;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using WhiteSparrow.Integrations.QC.LogDetails;
using Object = UnityEngine.Object;

namespace WhiteSparrow.Integrations.QC
{
	public static class ChirpQuantumConsoleConverter
	{
		[MenuItem("CONTEXT/QuantumConsole/Convert to Chirp")]
		private static void ConvertQuantumConsole(MenuCommand command)
		{
			QuantumConsole source = command.context as QuantumConsole;
			if (source == null)
				return;

			ChirpQuantumConsole chirpConsole = null;
			var allQuantumConsoleReferences = FindAllReferences(source);
			var consoleReplacement = ReplaceQuantumConsole(source, out chirpConsole);
			ReplaceAllReferences(allQuantumConsoleReferences, chirpConsole);
			var logSelector = AddLogSelectorComponent(chirpConsole);
			
			EditorUtility.DisplayDialog("Chirp: Convert Quantum Console",
				$"Quantum console update: {consoleReplacement},\nLog selector creation: {logSelector}", "Ok");
		}

		private enum ConsoleReplacementResult
		{
			Success,
			AlreadyExists
		}

		private static ConsoleReplacementResult ReplaceQuantumConsole(QuantumConsole source, out ChirpQuantumConsole chirpQuantumConsole)
		{
			if (source is ChirpQuantumConsole sourceAsChirp)
			{
				chirpQuantumConsole = sourceAsChirp;
				return ConsoleReplacementResult.AlreadyExists;
			}

			GameObject sourceGameObject = source.gameObject;
			
			Component[] allComponent = sourceGameObject.GetComponents<Component>();
			int componentCount = allComponent.Length;
			int componentIndex = Array.IndexOf(allComponent, source);
			

			GameObject temporaryGameObject = new GameObject();
			ChirpQuantumConsole temporaryQuantumConsole = temporaryGameObject.AddComponent<ChirpQuantumConsole>();

			EditorUtility.CopySerialized(source, temporaryQuantumConsole);

			if (Application.isPlaying)
			{
				Object.Destroy(source);
			}
			else
			{
				Object.DestroyImmediate(source);
			}
			
			chirpQuantumConsole = sourceGameObject.AddComponent<ChirpQuantumConsole>();
			EditorUtility.CopySerialized(temporaryQuantumConsole, chirpQuantumConsole);
			
			var prefabStatus = PrefabUtility.GetPrefabInstanceStatus(sourceGameObject);
			if (prefabStatus == PrefabInstanceStatus.NotAPrefab)
			{
				
				if (componentIndex != -1 && componentCount - 1 > componentIndex)
				{
					for (int i = componentCount - 1; i >= componentIndex; i--)
					{
						UnityEditorInternal.ComponentUtility.MoveComponentUp(chirpQuantumConsole);
					}
				}
			}
			
			if (Application.isPlaying)
			{
				Object.Destroy(temporaryGameObject);
			}
			else
			{
				Object.DestroyImmediate(temporaryGameObject);
			}

			return ConsoleReplacementResult.Success;
		}

		private enum LogSelectorResult
		{
			Success,
			Updated,
			UnableToAdd
		}
		private static LogSelectorResult AddLogSelectorComponent(ChirpQuantumConsole chirpConsole)
		{
			if (chirpConsole == null)
				return LogSelectorResult.UnableToAdd;
			
			SerializedObject so = new SerializedObject(chirpConsole);
			SerializedProperty _consoleLogText = so.FindProperty("_consoleLogText");
			TextMeshProUGUI consoleTextContainer = _consoleLogText.objectReferenceValue as TextMeshProUGUI;

			bool createdNew = false;
			
			var allLogSelectorsFound = chirpConsole.GetComponentsInChildren<QuantumConsoleLogSelector>();
			QuantumConsoleLogSelector targetLogSelector = null;
			if (allLogSelectorsFound.Length > 1)
			{
				for (int i = 0; i < allLogSelectorsFound.Length; i++)
				{
					if(Application.isPlaying)
						Object.Destroy(allLogSelectorsFound[i]);
					else
						Object.DestroyImmediate(allLogSelectorsFound[i]);
				}
			}
			else if (allLogSelectorsFound.Length == 1)
			{
				targetLogSelector = allLogSelectorsFound[0];
			}
			else
			{
				// no log selector found, we need to create one
				ScrollRect consoleTextScrollRect = consoleTextContainer.GetComponentInParent<ScrollRect>();
				RectTransform targetTransform = consoleTextScrollRect.viewport;

				targetLogSelector = targetTransform.gameObject.AddComponent<QuantumConsoleLogSelector>();
				createdNew = true;
			}

			if (targetLogSelector == null)
			{
				return LogSelectorResult.UnableToAdd;
			}

			SerializedObject targetSo = new SerializedObject(targetLogSelector);
			SerializedProperty _textComponent = targetSo.FindProperty("_textComponent");
			SerializedProperty _chirpQuantumConsole = targetSo.FindProperty("_chirpQuantumConsole");

			_textComponent.objectReferenceValue = consoleTextContainer;
			_chirpQuantumConsole.objectReferenceValue = chirpConsole;

			targetSo.ApplyModifiedPropertiesWithoutUndo();

			return createdNew ? LogSelectorResult.Success : LogSelectorResult.Updated;
		}


		#region Reference Replacement

		
		private static SerializedProperty[] FindAllReferences(QuantumConsole source)
		{
			// already replaced
			if (source is ChirpQuantumConsole)
				return Array.Empty<SerializedProperty>();
			
			List<SerializedProperty> output = new List<SerializedProperty>();
			Component[] allComponents = source.GetComponentsInChildren<Component>();
			foreach (var component in allComponents)
			{
				if (component == source)
					continue;

				SerializedObject so = new SerializedObject(component);
				SerializedProperty iterator = so.GetIterator();
				while (iterator.Next(true))
				{
					if (iterator.propertyType != SerializedPropertyType.ObjectReference)
						continue;

					if (iterator.objectReferenceValue != source)
						continue;
					
					output.Add(iterator.Copy());
				}
			}

			return output.ToArray();
		}

		private static void ReplaceAllReferences(IEnumerable<SerializedProperty> referenceList, QuantumConsole target)
		{
			if (referenceList == null || target == null)
				return;
			
			foreach (var pointer in referenceList)
			{
				pointer.objectReferenceValue = target;
				pointer.serializedObject.ApplyModifiedPropertiesWithoutUndo();
			}
		}


		#endregion
	}
}